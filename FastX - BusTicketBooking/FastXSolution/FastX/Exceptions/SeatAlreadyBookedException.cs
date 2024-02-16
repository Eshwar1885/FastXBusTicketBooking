namespace FastX.Exceptions
{
    [Serializable]
    public class SeatAlreadyBookedException : Exception
    {
        string message;
        public SeatAlreadyBookedException()
        {
            message = "This seat is already boooked.";
        }
        public override string Message => message;

    }
}
