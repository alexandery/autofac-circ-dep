using Sample.Services;

namespace Sample
{
    public class MyWorker
    {
        public IService service;
        public ISomeDependency dependency;

        /// <summary>
        /// Needs both - IService as well as a dependency instance.
        /// Should not be a problem, since IService instance has been pre-resolved and should be already available in Autofac lifetime scope - 
        /// should be simple passing of that IService instance, as well as using same IService instance when creating an instance of ISomeDependency
        /// </summary>
        public MyWorker(IService service, ISomeDependency dependency)
        {
            this.service = service;
            this.dependency = dependency;
        }

        public void InvokeAction()
        {
            service.DoStuff();
        }
    }
}
