using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bns.Domain.Users;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //builder.Property(s => s.AccessFailedCount).HasColumnName("access_failder_count");

        //builder.Property(s => s.ConcurrencyStamp).HasColumnName("concurrency_stamp");
        //builder.Property(s => s.Email).HasColumnName("email");
        //builder.Property(s => s.EmailConfirmed).HasColumnName("email_confirmed");
        //builder.Property(s => s.FullName).HasColumnName("full_name");
        //builder.Property(s => s.Id).HasColumnName("id");
        //builder.Property(s => s.IsActive).HasColumnName("is_active").IsRequired();
        //builder.Property(s => s.LockoutEnabled).HasColumnName("lockout_enabled");
        //builder.Property(s => s.LockoutEnd).HasColumnName("lockout_end");
        //builder.Property(s => s.ModifyTime).HasColumnName("modify_time");
        //builder.Property(s => s.PhoneNumber).HasColumnName("phone_number");
        //builder.Property(s => s.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
        //builder.Property(s => s.SecurityStamp).HasColumnName("security_stamp");
        //builder.Property(s => s.TwoFactorEnabled).HasColumnName("two_factor_enabled");
        //builder.Property(s => s.UserName).HasColumnName("user_name");
        //builder.Property(s => s.CreateTime).HasColumnName("create_time").HasDefaultValueSql("CURRENT_TIMESTAMP");
        //builder.Property(s => s.DeleteTime).HasColumnName("delete_time");
        //builder.Property(s => s.SettingsId).HasColumnName("settings_id");

        // Uncomment if you want to use a custom value converter for Email


        //builder.Property(s => s.Email).HasConversion(new EmailValueConverter());

        //builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        //builder.Property(u => u.PasswordHash).IsRequired();
        //builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);
        //builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
        //builder.Property(u => u.Email).HasConversion<string>();
        //builder.Property(u => u.CreateTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}