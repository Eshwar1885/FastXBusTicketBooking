using System.Runtime.Serialization;

namespace FastX.Exceptions
{
    [Serializable]
    public class NoTicketsAvailableException : Exception
    {
        public NoTicketsAvailableException()
        {
        }

        public NoTicketsAvailableException(string? message) : base(message)
        {
        }

        public NoTicketsAvailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoTicketsAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}