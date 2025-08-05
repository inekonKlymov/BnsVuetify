using Bns.Domain.Abstracts;

namespace Bns.Domain.Users;

public readonly struct LanguageId(int value) : IStrongTypeId<int>
{
    public int Value { get; } = value;
    public override string ToString() => Value.ToString();

    public static implicit operator int(LanguageId id) => id.Value;

    public static explicit operator LanguageId(int value) => new(value);
}
