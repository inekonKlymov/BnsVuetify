namespace Bns.Domain.Common.Startup;

public class AppSettings
{
    public AppSettingsConnectionStrings ConnectionStrings { get; set; }
    public JwtSettings Jwt { get; set; }
}

public record AppSettingsConnectionStrings(string DefaultConnection, string ClickHouse);

/// <param name="Issuer"></param>
/// <param name="Audience"></param>
/// <param name="RefreshTokenTTL"> Gets or sets the refresh token TTL in days </param>
public record JwtSettings(string Issuer, string Audience, uint RefreshTokenTTL);
