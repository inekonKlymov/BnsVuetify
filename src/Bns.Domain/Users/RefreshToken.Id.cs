using Bns.Domain.Abstracts;

namespace Bns.Domain.Users;

public readonly struct RefreshTokenId(Guid value) : IStrongTypeId<Guid>
{
    public Guid Value { get; } = value;
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(RefreshTokenId email) => email.Value;
    public static explicit operator RefreshTokenId(Guid value) => new(value);
}
