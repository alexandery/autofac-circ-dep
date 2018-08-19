using System;

namespace Sample.Services
{
    public class SimpleService : ISimpleService
    {

        public delegate SimpleService Factory(Guid id);

        private Guid id;

        public SimpleService(Guid id)
        {
            this.id = id;
        }

        #region Implementation of IService

        /// <inheritdoc />
        public void DoStuff()
        {
            Console.WriteLine("This is SimpleService doing stuff");
        }

        /// <inheritdoc />
        public void Filler()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
