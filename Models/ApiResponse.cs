namespace ControleEstoqueApi.Models;

public class ApiResponse<T> {
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }

    public ApiResponse() { }

    public ApiResponse(bool success, string message, T? data = default, Dictionary<string, string[]>? errors = null) {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors;
    }

    public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso.") =>
        new ApiResponse<T>(true, message, data);

    public static ApiResponse<T> BadRequest(string message, Dictionary<string, string[]>? errors = null) =>
        new ApiResponse<T>(false, message, errors: errors);

    public static ApiResponse<T> NotFound(string message = "Recurso não encontrado.") =>
        new ApiResponse<T>(false, message);

    public static ApiResponse<T> ServerError(string message = "Erro interno do servidor.") =>
        new ApiResponse<T>(false, message);

    public static ApiResponse<object> Ok(string message = "Operação realizada com sucesso.") =>
        new ApiResponse<object>(true, message, new object());

    public static ApiResponse<object> BadRequest(string message, Dictionary<string, string[]>? errors = null) =>
        new ApiResponse<object>(false, message, errors: errors);

    public static ApiResponse<object> NotFound(string message = "Recurso não encontrado.") =>
        new ApiResponse<object>(false, message);

    public static ApiResponse<object> ServerError(string message = "Erro interno do servidor.") =>
        new ApiResponse<object>(false, message);
}
