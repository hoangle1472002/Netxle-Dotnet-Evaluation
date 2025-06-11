using System;

namespace NexleEvaluation.Domain.Interfaces
{
    public interface ICustomTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
