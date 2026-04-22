using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDo.WebAPI.Filters
{
    public class ModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                context.Result = new BadRequestObjectResult(new
                {
                    message = string.Join(" | ", errors),
                    errors = errors.ToList()
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

