namespace OnlineStore.Exceptions
{
    public class NotEnoughStock : Exception
    {
        public NotEnoughStock(string message)
            : base(message)
        {
        }

        // Optionally, you can add additional constructors if needed
        public NotEnoughStock(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
