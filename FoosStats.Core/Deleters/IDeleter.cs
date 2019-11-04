using System;

namespace FoosStats.Core.Deleters
{
    public interface IDeleter<T>
    {
        void Delete(Guid foosElementID);
    }
}
