namespace OnlineStore.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string message)
            : base(message)
        {
        }

        // Optionally, you can add additional constructors if needed
        public UserAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
