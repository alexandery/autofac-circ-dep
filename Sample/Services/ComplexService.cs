using System;

namespace Sample.Services
{
    public class ComplexService : IComplexService
    {
        public delegate ComplexService Factory(Guid id);

        private Guid id;

        public ComplexService(Guid id)
        {
            this.id = id;
        }

        #region Implementation of IService

        /// <inheritdoc />
        public void DoStuff()
        {
            Console.WriteLine("This is ComplexService doing stuff");
        }

        /// <inheritdoc />
        public void AnotherAction()
        {
            Console.WriteLine("Some other action");
        }

        #endregion
    }
}
