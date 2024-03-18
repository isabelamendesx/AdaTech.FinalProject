using Microsoft.AspNetCore.Mvc.Filters;

namespace Model.Application.API.Filters.Model
{
    public class ErrorResponseBuilder
    {
        private ErrorResponse _errorResponse;

        public ErrorResponseBuilder() 
        {
            _errorResponse = new ErrorResponse();
        }

        public ErrorResponseBuilder WithDefaultProperties(ExceptionContext context)
        {
            _errorResponse.Message = context.Exception.Message;
            _errorResponse.TimeStamp = DateTime.UtcNow;
            _errorResponse.RequestPath = context.HttpContext.Request.Path;
            return this;
        }

        public ErrorResponseBuilder WithTitle(string title)
        {
            _errorResponse.Title = title;
            return this;
        }

        public ErrorResponseBuilder WithStatusCode(int statusCode)
        {
            _errorResponse.StatusCode = statusCode;
            return this;
        }

        public ErrorResponseBuilder WithConflictingRuleId(uint conflictingRuleId)
        {
            _errorResponse.ConflictingRuleId = conflictingRuleId;
            return this;
        }
          public ErrorResponseBuilder WithConflictingCategoryId(uint conflictingCategoryId)
        {
            _errorResponse.ConflictingCategoryId = conflictingCategoryId;
            return this;
        }

        public ErrorResponseBuilder WithDetails(string details)
        {
            _errorResponse.Details = details;
            return this;
        }

        public void WithValidationErrors(IDictionary<string, string[]> Errors)
        {
            _errorResponse.Errors = Errors;
        }

        public ErrorResponse Build()
        {
            return _errorResponse;
        }

    }
}
