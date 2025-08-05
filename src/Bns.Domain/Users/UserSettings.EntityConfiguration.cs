using Bns.Domain.Common.DatabaseConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bns.Domain.Users;

public class UserSettingsEntityConfiguration : IEntityTypeConfiguration<UserSettings>
{
    public void Configure(EntityTypeBuilder<UserSettings> builder)
    {
        builder.Property(x => x.Skin).HasConversion<string>();
        builder.Property(x => x.DimensionViewDisplayType).HasConversion<string>();
        builder.Property(x => x.UseCurrentDate).HasConversion(new JsonValueConverter<UseCurrentDate>());
        builder.Property(x => x.DimensionPresets).HasConversion(new JsonValueConverter<List<DimensionPreset>>());
    }
}
