namespace Messaging.Exceptions;

public class UserExistsException(string message) : Exception(message);