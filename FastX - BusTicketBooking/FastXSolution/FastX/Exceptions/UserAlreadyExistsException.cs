namespace FastX.Exceptions
{
    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        string message;
        public UserAlreadyExistsException()
        {
            message = "User already exists";
        }
        public override string Message => message;

    }
}
