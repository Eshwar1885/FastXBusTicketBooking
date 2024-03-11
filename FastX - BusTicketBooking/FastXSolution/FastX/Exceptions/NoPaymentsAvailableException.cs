namespace FastX.Exceptions
{
    public class NoPaymentsAvailableException : Exception
    {
        string message;
        public NoPaymentsAvailableException()
        {
            message = "No payment is available";
        }
        public override string Message => message;
    }
}
