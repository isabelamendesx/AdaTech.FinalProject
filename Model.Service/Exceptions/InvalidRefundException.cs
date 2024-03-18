namespace Model.Service.Exceptions
{
    public class InvalidRefundException : Exception
    {
        public int StatusCode { get; set; }

        public InvalidRefundException(string message) : base(message)
        {
            StatusCode = 400;
        }
    }
}
