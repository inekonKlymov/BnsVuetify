namespace Bns.Domain.Users;

public record PresetGroup
    (
    string Name,
    string DataSource,
    List<string> Dimensions,
    PresetGroupType Type = PresetGroupType.Other
    );
