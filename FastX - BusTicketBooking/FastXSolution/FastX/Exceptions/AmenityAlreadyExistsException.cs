using System.Runtime.Serialization;

namespace FastX.Exceptions
{
    [Serializable]
    internal class AmenityAlreadyExistsException : Exception
    {
        public AmenityAlreadyExistsException()
        {
        }

        public AmenityAlreadyExistsException(string? message) : base(message)
        {
        }

        public AmenityAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AmenityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}