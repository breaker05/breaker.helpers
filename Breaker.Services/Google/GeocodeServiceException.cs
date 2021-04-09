using System;

namespace Breaker.Services.Google
{
    public class GeocodeServiceException : Exception
    {
        public GeocodeServiceException()
        {
        }

        public GeocodeServiceException(string message)
            : base(message)
        {
        }

        public GeocodeServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
