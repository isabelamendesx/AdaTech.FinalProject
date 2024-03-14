using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.Application.API.Filters.Model;
using Model.Service.Exceptions;
using Newtonsoft.Json;
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
                .WithDefaultProperties(context);

            if (context.Exception is RuleException ruleEx)
                errorResultBuilder
                   .WithConflictingRuleId(ruleEx.ConflictingRuleId)
                   .WithDetails(ruleEx.Details);

            var errorResult = errorResultBuilder.Build();
            var ignoreNullFields = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

            context.Result = new JsonResult(errorResult, ignoreNullFields)
            {
                StatusCode = errorResult.StatusCode
            };

            _logger.LogError(context.Exception, "Error caught in exception filter.");
        }
    }
}
