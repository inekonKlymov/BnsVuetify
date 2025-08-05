using Bns.Domain.Abstracts;

namespace Bns.Domain.Users;

public readonly struct UserSettingsId(Guid value) : IStrongTypeId<Guid>
{
    public Guid Value { get; } = value;

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserSettingsId id) => id.Value;

    public static explicit operator UserSettingsId(Guid value) => new(value);
}
