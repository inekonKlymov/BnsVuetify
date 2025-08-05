using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bns.Domain.Users;

public class EmailValueConverter : ValueConverter<Email, string>
{
    public EmailValueConverter() : base(
        v => v.Value,
        v => new Email(v))
    { }
}