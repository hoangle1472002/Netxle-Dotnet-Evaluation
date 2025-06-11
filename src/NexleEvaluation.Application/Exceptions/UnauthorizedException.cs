using System;

namespace NexleEvaluation.Application.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) :
            base(message)
        {

        }
    }
}