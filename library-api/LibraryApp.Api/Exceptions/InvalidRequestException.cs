namespace LibraryApp.Api.Exceptions;

public sealed class InvalidRequestException(string message)
    : AppException(message, StatusCodes.Status400BadRequest);
