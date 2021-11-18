using System;
using System.Runtime.Serialization;

namespace RepairMarketPlace.ApplicationCore.Exceptions
{
    public class SingleShopException : Exception
    {
        public SingleShopException(string email) : base($"User with email {email} already has a Shop.") 
        {
        }

        public SingleShopException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SingleShopException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
