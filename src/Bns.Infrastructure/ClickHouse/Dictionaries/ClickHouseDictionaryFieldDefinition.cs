namespace Bns.Infrastructure.ClickHouse.Dictionaries;

public class ClickHouseDictionaryFieldDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? DefaultOrExpression { get; set; }
    public string? DefaultOrExpressionValue { get; set; }
    public bool IsObjectId { get; set; } = false;
    public bool Hierarchical { get; set; } = false;
    public bool Injective { get; set; } = false;

    public override string ToString()
    {
        var col = Name + " " + Type;
        if (!string.IsNullOrWhiteSpace(DefaultOrExpression) && !string.IsNullOrWhiteSpace(DefaultOrExpressionValue))
            col += $" {DefaultOrExpression} {DefaultOrExpressionValue}";
        if (IsObjectId)
            col += " IS_OBJECT_ID";
        if (Hierarchical)
            col += " HIERARCHICAL";
        if (Injective)
            col += " INJECTIVE";
        return col;
    }
}
