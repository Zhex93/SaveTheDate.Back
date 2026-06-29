using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Api.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var (statusCode, message) = context.Exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, context.Exception.Message),
            InvalidOperationDomainException => (HttpStatusCode.BadRequest, context.Exception.Message),
            Domain.Common.ValidationException => (HttpStatusCode.BadRequest, context.Exception.Message),
            FluentValidation.ValidationException ve => (HttpStatusCode.BadRequest,
                string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            _ => (HttpStatusCode.InternalServerError, "An internal error occurred.")
        };

        context.Result = new ObjectResult(new { error = message })
        {
            StatusCode = (int)statusCode
        };
        context.ExceptionHandled = true;
    }
}
