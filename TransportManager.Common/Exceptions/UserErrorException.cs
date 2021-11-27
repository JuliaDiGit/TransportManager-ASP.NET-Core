using System;

namespace TransportManager.Common.Exceptions
{
    public class UserErrorException : Exception
    {
        public UserErrorException(string message) : base(message)
        {
        }
    }
}
