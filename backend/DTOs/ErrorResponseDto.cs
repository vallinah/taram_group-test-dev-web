namespace backend.DTOs;

public class ErrorResponseDto
{
    public string Message { get; set; } = null!;

    public object? Details { get; set; }
}