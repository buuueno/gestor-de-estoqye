using System.Net;
using System.Text.Json;
using ControleEstoqueApi.Models;

namespace ControleEstoqueApi.Middleware;

public class ErrorHandlingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        } catch (Exception ex) {
            _logger.LogError(ex, "Erro não capturado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
        context.Response.ContentType = "application/json";
        
        var response = new ApiResponse<object>();

        switch (exception) {
            case ArgumentException argEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = ApiResponse<object>.BadRequest(argEx.Message);
                break;

            case KeyNotFoundException keyEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response = ApiResponse<object>.NotFound(keyEx.Message);
                break;

            case InvalidOperationException invalidEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = ApiResponse<object>.BadRequest(invalidEx.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = ApiResponse<object>.ServerError($"Erro interno do servidor: {exception.Message}");
                break;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsJsonAsync(response, options);
    }
}
