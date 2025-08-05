namespace Bns.Domain.Users;

public record DimensionPreset(
    string GroupKey,
    bool UserPanels,
    bool ImplicitPanels,
    PresetGroup PresetGroup,
    string SelectedMember = "defaultMember"
    );
