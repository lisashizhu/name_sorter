using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NameSorter.Common.Exceptions;

namespace NameSorter.WebApi.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessException || context.Exception is ArgumentException)
            {
                context.Result = new BadRequestObjectResult(new {@error = context.Exception.Message});
            }
        }
    }
}
