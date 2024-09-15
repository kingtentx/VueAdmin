﻿using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Threading.Tasks;

namespace VueAdmin.Web.Filters
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
