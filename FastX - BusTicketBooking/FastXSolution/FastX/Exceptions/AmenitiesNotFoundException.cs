namespace FastX.Exceptions
{
    [Serializable]
    public class AmenitiesNotFoundException : Exception
    {
        string message;
        public AmenitiesNotFoundException()
        {
            message = "No amenities found for the specified bus.";
        }
        public override string Message => message;

    }
}
