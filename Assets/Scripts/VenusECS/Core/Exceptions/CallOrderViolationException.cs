using System;

namespace VenusECS.Core.Exceptions
{
    public class CallOrderViolationException : Exception
    {
        public CallOrderViolationException(string message) : base(message)
        {
            
        }
    }
}