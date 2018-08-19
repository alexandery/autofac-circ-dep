using System;

namespace Sample.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly SimpleService.Factory simpleServiceFactoryDelegate;
        private readonly ComplexService.Factory complexServiceFactoryDelegate;

        public ServiceFactory(SimpleService.Factory simpleServiceFactoryDelegate, ComplexService.Factory complexServiceFactoryDelegate)
        {
            this.simpleServiceFactoryDelegate = simpleServiceFactoryDelegate;
            this.complexServiceFactoryDelegate = complexServiceFactoryDelegate;
        }

        public IComplexService CreateComplexService(Guid id)
        {
            return complexServiceFactoryDelegate(id);
        }

        public ISimpleService CreateSimpleService(Guid id)
        {
            return simpleServiceFactoryDelegate(id);
        }
    }
}
