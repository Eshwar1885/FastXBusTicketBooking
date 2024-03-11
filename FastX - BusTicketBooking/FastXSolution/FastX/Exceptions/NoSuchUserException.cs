using System.Runtime.Serialization;

namespace FastX.Exceptions
{
    [Serializable]
    public class NoSuchUserException : Exception
    {
        string message;
        public NoSuchUserException()
        {
            message = "No such user exists";
        }
        public override string Message => message;

    }
}