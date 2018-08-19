using System;

namespace Sample.Services
{
    public interface IServiceFactory
    {
        IComplexService CreateComplexService(Guid id);
        ISimpleService CreateSimpleService(Guid id);
    }
}