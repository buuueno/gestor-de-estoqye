using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ControleEstoqueApi.Models;

namespace ControleEstoqueApi.Middleware;

public class ValidationFilter : IActionFilter {
    public void OnActionExecuting(ActionExecutingContext context) {
        if (!context.ModelState.IsValid) {
            var errors = new Dictionary<string, string[]>();
            
            foreach (var kvp in context.ModelState) {
                var errorMessages = kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>();
                if (errorMessages.Length > 0) {
                    errors[kvp.Key] = errorMessages;
                }
            }

            var response = ApiResponse<object>.BadRequest("Erro de validação.", errors);
            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
