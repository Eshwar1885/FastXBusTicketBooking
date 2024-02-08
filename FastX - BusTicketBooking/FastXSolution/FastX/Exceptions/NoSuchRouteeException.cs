namespace FastX.Exceptions
{
    public class NoSuchRouteeException : Exception
    {
        string message;
        public NoSuchRouteeException()
        {
            message = "No such routee is available";
        }
        public override string Message => message;

    }
}
