using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Abstractions;
using NOCIL_VP.API.Logging;
using NOCIL_VP.Domain.Core.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Filters
{
    public class CommonExceptionHandler : IAsyncExceptionFilter
    {

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var ai = context.HttpContext.RequestServices.GetService(typeof(TelemetryClient)) as TelemetryClient;
            var status = HttpStatusCode.InternalServerError;
            var message = string.Empty;

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if(exceptionType == typeof(ArgumentException))
            {
                message = "Values can not be empty, Fill out all the required details";
                status = HttpStatusCode.UnprocessableEntity;
            }
            else
            {
                message = context.Exception.Message;
                status = HttpStatusCode.BadRequest;
            }

            LogWritter.WriteErrorLog(context.Exception);

            context.ExceptionHandled = true;

            var response = context.HttpContext.Response;

            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            ResponseMessage exception = new ResponseMessage();
            exception.Message = message;
            exception.Error = context.Exception.Message;
            exception.Status = (int)status;

            await response.WriteAsync(JsonSerializer.Serialize(exception));
        }
    }
}
