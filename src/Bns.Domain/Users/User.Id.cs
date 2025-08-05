using Bns.Domain.Abstracts;

namespace Bns.Domain.Users;

public readonly struct UserId(Guid value) : IStrongTypeId<Guid>, IEquatable<UserId>
{
    public Guid Value { get; } = value;

    public bool Equals(UserId other)
    {
        return Value.Equals(other.Value);
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserId id) => id.Value;

    public static explicit operator UserId(Guid value) => new(value);
}
