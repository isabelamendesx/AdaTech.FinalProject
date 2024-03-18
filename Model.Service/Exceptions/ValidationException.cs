using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Model.Service.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public int StatusCode { get; set; }

        public ValidationException(ModelStateDictionary modelState)
       : base("Validation failed. See 'Errors' property for details.")
        {
            StatusCode = 422;
            Errors = modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }

    }
}
