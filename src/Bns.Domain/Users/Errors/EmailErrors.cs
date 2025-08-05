using Bns.Domain.Common.Errors;
using FluentResults;

namespace Bns.Domain.Users.Errors;
public static class EmailErrors
{
    public static readonly ValidationError InvalidEmailFormat = new("The email format is invalid.");
    public static readonly ValidationError EmptyEmail = new("Email cannot be empty.");
    public static readonly ValidationError EmailTooLong = new("Email is too long.");
   
}
public static class UserErrors
{
    public static readonly NotFoundError UserNotFound = new("User not found.");
    public static readonly DomainError UserAlreadyExists = new("User already exists.");
    public static readonly DomainError InvalidPassword = new("Invalid password.");
    public static readonly ValidationError PasswordTooShort = new("Password must be at least 8 characters long.");
    public static readonly ValidationError PasswordTooLong = new("Password cannot exceed 100 characters.");
    public static readonly UnauthorizedError UserLoginFail = new("Error while login. Wrong login or password.");
    public static readonly DomainError TokenIsNull = new("Token can not be null");
    public static readonly DomainError ValidationTokenError = new("Token validation error. Token is not valid or expired.");
    public static readonly IError InvalidToken;
}