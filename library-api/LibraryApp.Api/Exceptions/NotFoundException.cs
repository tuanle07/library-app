namespace LibraryApp.Api.Exceptions;

public sealed class NotFoundException(string message)
    : AppException(message, StatusCodes.Status404NotFound);
