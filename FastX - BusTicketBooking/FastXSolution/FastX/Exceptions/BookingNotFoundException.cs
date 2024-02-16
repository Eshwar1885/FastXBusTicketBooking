using System.Runtime.Serialization;

namespace FastX.Exceptions
{
    [Serializable]
    internal class BookingNotFoundException : Exception
    {
        public BookingNotFoundException()
        {
        }

        public BookingNotFoundException(string? message) : base(message)
        {
        }

        public BookingNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BookingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}