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
                var fieldErrors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                context.Result = new BadRequestObjectResult(new
                {
                    message = "Validation failed",
                    errors = fieldErrors
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

