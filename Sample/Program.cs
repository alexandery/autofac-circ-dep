using System;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Sample.Services;

namespace Sample
{
    class Program
    {
        private IContainer container;

        static void Main(string[] args)
        {
            var program = new Program();

            program.PrepareContainer();

            program.Action();

            Console.ReadLine();
        }

        public void PrepareContainer()
        {
            var builder = new ContainerBuilder();

            // registering services and a factory to manage service creation based on particular parameters
            builder.RegisterType<SimpleService>().As<ISimpleService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterGeneratedFactory<SimpleService.Factory>(new TypedService(typeof(SimpleService)));
            builder.RegisterType<ComplexService>().As<IComplexService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterGeneratedFactory<ComplexService.Factory>(new TypedService(typeof(ComplexService)));
            // custom factory allowing creation of a specific type
            builder.RegisterType<ServiceFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();

            // user code always references IService - specializations are managed at a lower level and not important to user code
            // so, resolution always looks for whether there is a specialized instance already resolved
            builder.Register<IService>(c =>
            {
                try { return c.Resolve<ISimpleService>(); } catch (Exception) { }
                try { return c.Resolve<IComplexService>(); } catch (Exception) { }
                var factory = c.Resolve<SimpleService.Factory>();
                return factory.Invoke(Guid.Empty);
            });
            // Lifecycle management is not specified for IService, because it only depends on lifecycle provided for each of ISimpleService and IComplexService
            //.InstancePerLifetimeScope();

            builder.RegisterType<SomeDependency>().As<ISomeDependency>().InstancePerLifetimeScope();
            builder.RegisterType<MyWorker>().AsSelf().InstancePerLifetimeScope();

            container = builder.Build();
        }

        public void Action()
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var serviceFactory = scope.Resolve<IServiceFactory>();
                // lower level code always makes a decision which concrete service to assign to a given lifetime scope
                // since the lifecycle is InstancePerLifetimeScope - as long as any implementation is resolved, it should be available to all users in a given scope
                
                // as long as IComplexService resolution attempt is NOT on line 41, but on line 40 - resolution of MyWorker below will work correctly
                // currently IComplextService resolution attempt follows ISimpleService resolution attempt (which throws an exception) - the instance of IComplexService gets returned properly
                // but MyWorker below will not get resolved correctly
                var service = serviceFactory.CreateComplexService(Guid.NewGuid());

                // Uncomment this (and comment "serviceFactory.CreateComplexService(Guid.NewGuid());" above) to observe correct resolution of everything
                //var service = serviceFactory.CreateSimpleService(Guid.NewGuid());

                // This throws circular dependendy exception if:
                // - service type resolved above is *not the first* that is tryinig to be resolved by lambda registration
                // - seems that a DependencyResolutionException that gets thrown in this situation by first "c.Resolve<>" somehow affects the behavior of the container
                var worker = scope.Resolve<MyWorker>();
                worker.InvokeAction();
            }
        }
    }
}
