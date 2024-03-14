using Microsoft.AspNetCore.Mvc.Filters;
using Model.Service.Exceptions;
using System.Net;

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
            _errorResponse.Title = GetTitle(context.Exception);
            _errorResponse.Message = context.Exception.Message;
            _errorResponse.TimeStamp = DateTime.UtcNow;
            _errorResponse.RequestPath = context.HttpContext.Request.Path;
            _errorResponse.StatusCode = GetStatusCode(context.Exception);
            return this;
        }

        public ErrorResponseBuilder WithConflictingRuleId(uint conflictingRuleId)
        {
            _errorResponse.ConflictingRuleId = conflictingRuleId;
            return this;
        }

        public ErrorResponseBuilder WithDetails(string details)
        {
            _errorResponse.Details = details;
            return this;
        }
        public ErrorResponse Build()
        {
            return _errorResponse;
        }

        private int GetStatusCode<T>(T ex) where T : Exception
        {
            var statusCodeMap = new Dictionary<Type, Func<int>>
            {
                { typeof(RuleException), () => (ex as RuleException)!.StatusCode },
                { typeof(ResourceNotFoundException), () => (ex as ResourceNotFoundException)!.StatusCode},
                { typeof(InternalErrorException), () => (ex as InternalErrorException)!.StatusCode },
                { typeof(CategoryAlreadyRegisteredException), () => (ex as CategoryAlreadyRegisteredException)!.StatusCode},
            };

            return statusCodeMap.TryGetValue(ex.GetType(), out var statusCodeFunc) ? statusCodeFunc() : (int)HttpStatusCode.InternalServerError;
        }

        private string GetTitle<T>(T ex) where T : Exception
        {
            var titleMap = new Dictionary<Type, string>
            {
                { typeof(RuleException), "Rule Operation Error" },
                { typeof(ResourceNotFoundException), "Resource Error" },
                { typeof(InternalErrorException), "Internal Error" },
                { typeof(CategoryAlreadyRegisteredException), "Category Operation Error" },
            };

            return titleMap.TryGetValue(ex.GetType(), out var title) ? title : "Internal Server Error";
        }

    }
}
