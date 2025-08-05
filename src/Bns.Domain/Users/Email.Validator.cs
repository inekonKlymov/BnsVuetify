using Bns.Domain.Users.Errors;
using FluentValidation;

namespace Bns.Domain.Users;

public class EmailValidator : AbstractValidator<Email>
{
    public EmailValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty().WithMessage(EmailErrors.EmptyEmail.Message)
            .MaximumLength(100).WithMessage(EmailErrors.EmailTooLong.Message)
            .EmailAddress().WithMessage(EmailErrors.InvalidEmailFormat.Message);
    }
}
