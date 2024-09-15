using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace VueAdmin.Api.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            Log.Error(context.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
