namespace Bns.Infrastructure.ClickHouse.Columns;

public record ClickHouseColumnDefinition(string? Nullability, string? Modifier, string? Expr, string? Comment, string? Codec, string? Ttl, string Name = "", string Type = "")
{

    public override string ToString()
    {
        var col = Name + " " + Type;
        if (!string.IsNullOrWhiteSpace(Nullability))
            col += " " + Nullability;
        if (!string.IsNullOrWhiteSpace(Modifier) && !string.IsNullOrWhiteSpace(Expr))
            col += $" {Modifier} {Expr}";
        if (!string.IsNullOrWhiteSpace(Comment))
            col += $" COMMENT '{Comment.Replace("'", "''")}'";
        if (!string.IsNullOrWhiteSpace(Codec))
            col += $" CODEC({Codec})";
        if (!string.IsNullOrWhiteSpace(Ttl))
            col += $" TTL {Ttl}";
        return col;
    }
}
