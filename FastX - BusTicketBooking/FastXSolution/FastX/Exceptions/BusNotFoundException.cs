namespace FastX.Exceptions
{
    [Serializable]
    public class BusNotFoundException : Exception
    {
        string message;
        public BusNotFoundException()
        {
            message = "no bus found. An error occurred while searching for buses.";
        }
        public override string Message => message;

    }

}
