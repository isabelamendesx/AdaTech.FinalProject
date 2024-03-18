namespace Model.Service.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public int StatusCode { get; }

        public ResourceNotFoundException(string resourceName) : base($"{resourceName} not found.")
        {
            StatusCode = 404;
        }

    }
}
