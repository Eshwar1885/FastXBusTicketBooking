using System.Runtime.Serialization;

namespace FastX.Exceptions
{
    [Serializable]
    internal class BusOperatorNotFoundException : Exception
    {
        public BusOperatorNotFoundException()
        {
        }

        public BusOperatorNotFoundException(string? message) : base(message)
        {
        }

        public BusOperatorNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BusOperatorNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}