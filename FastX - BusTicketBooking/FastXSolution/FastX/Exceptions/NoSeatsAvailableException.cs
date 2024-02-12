using System.Runtime.Serialization;

namespace FastX.Exceptions
{
    [Serializable]
    internal class NoSeatsAvailableException : Exception
    {
        string message;
        public NoSeatsAvailableException()
        {
            message = "No Seats available at the moment";
        }

        public override string Message => message;
    }
}