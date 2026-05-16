namespace server.Application.Common.Exceptions;

public class AuthException : Exception
{
    public int StatusCode { get; }

    public AuthException(string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
    }
}
