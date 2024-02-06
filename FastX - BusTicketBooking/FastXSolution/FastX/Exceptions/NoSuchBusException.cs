namespace FastX.Exceptions
{
    [Serializable]
    public class NoSuchBusException : Exception
    {
        string message;
        public NoSuchBusException()
        {
            message = "No such bus exists";
        }
        public override string Message => message;

    }

}
