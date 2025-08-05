namespace Bns.Domain.Users;

public readonly struct Email
{
    public string Value { get; init; } 

    public Email(string value)
    {
        var validator = new EmailValidator();
        var result = validator.Validate(this with { Value = value });

        if (!result.IsValid)
            throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.ErrorMessage)), nameof(value));

        Value = value;
    }
    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string value) => new(value);
}
