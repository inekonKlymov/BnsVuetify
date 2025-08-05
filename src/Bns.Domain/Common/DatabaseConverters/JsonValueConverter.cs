using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Bns.Domain.Common.DatabaseConverters;
public class JsonValueConverter<T> : ValueConverter<T, string>
{
    public JsonValueConverter(JsonSerializerOptions? options = null)
        : base(
            v => JsonSerializer.Serialize(v, options ?? JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<T>(v, options ?? JsonSerializerOptions.Default) ?? default!
        )
    { }
}
