namespace HouseBroker.Application.CustomException;

public class BadRequestException : Exception
{
    public BadRequestException() { }
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception exception) : base(message, exception) { }
    public BadRequestException(List<string> errors) : base(string.Join(", ", errors)) { }
}