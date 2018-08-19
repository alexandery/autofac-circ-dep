using Sample.Services;

namespace Sample
{
    /// <summary>
    /// Some random dependency that needs a reference to IService for current Autofac scope. The instance of IService is pre-resolved for any given lifetime scope
    /// so should be always available and pre-created
    /// </summary>
    public class SomeDependency : ISomeDependency
    {
        private IService service;

        /// <summary>
        /// IService instance should be a pre-resolved, single instance in any given lifetime scope (based on lifecycle definition during registration)
        /// </summary>
        /// <param name="service"></param>
        public SomeDependency(IService service)
        {
            this.service = service;
        }
    }

    public interface ISomeDependency
    {
    }
}
