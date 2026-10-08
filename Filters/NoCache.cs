using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Project.Helpers
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Adiciona cabeçalhos HTTP que obrigam o navegador a revalidar a página toda vez
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";

            base.OnResultExecuting(context);
        }
    }
}
