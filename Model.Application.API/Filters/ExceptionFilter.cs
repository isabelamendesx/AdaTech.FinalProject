using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.Application.API.Filters.Model;
using Model.Service.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Model.Application.API.Filters
{
    public sealed class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var errorResultBuilder = new ErrorResponseBuilder()
                .WithDefaultProperties(context)
                .WithTitle(GetTitle(context.Exception))
                .WithStatusCode(GetStatusCode(context.Exception));

            if (context.Exception is RuleException ruleEx)
                errorResultBuilder
                   .WithConflictingRuleId(ruleEx.ConflictingRuleId)
                   .WithDetails(ruleEx.Details);

            if (context.Exception is CategoryAlreadyRegisteredException categoryEx)
                errorResultBuilder
                   .WithConflictingCategoryId(categoryEx.ConflictingCategoryId);

            var errorResult = errorResultBuilder.Build();
            var ignoreNullFields = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            context.Result = new JsonResult(errorResult, ignoreNullFields)
            {
                StatusCode = errorResult.StatusCode
            };

            _logger.LogError(context.Exception, "Error caught in exception filter.");
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
