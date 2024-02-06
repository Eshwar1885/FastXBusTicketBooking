namespace FastX.Exceptions
{
    public class NoSuchBusAvailableException:Exception
    {
        string message;
        public NoSuchBusAvailableException()
        {
            message = "No bus is available";
        }
        public override string Message => message;

    }
}
