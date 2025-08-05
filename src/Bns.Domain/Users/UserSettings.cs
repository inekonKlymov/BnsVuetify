using Bns.Domain.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bns.Domain.Users;

[Table("user_settings")]
public class UserSettings : Entity<UserSettingsId>
{
    public Language? Language { get; set; }  // Язык интерфейса

    [ForeignKey(nameof(Language))]
    [Column("language_id")]
    public LanguageId? LanguageId { get; set; }

    [Column("minimalize_menu")]
    public bool MinimalizeMenu { get; set; }

    [Column("undo_enabled")]
    public bool UndoEnabled { get; set; }

    [Column("load_notes")]
    public bool LoadNotes { get; set; }

    [Column("display_zeros")]
    public bool DisplayZeros { get; set; }

    [Column("supress_null_rows")]
    public bool SuppressNullRows { get; set; }
    [Column("highlight_active_row")]
    public bool HighlightActiveRow { get; set; }

    [Column("remove_null_rows_in_shanpshot")]
    public bool RemoveNullRowsInSnapshot { get; set; }

    [Column("prefer_user_panels")]
    public bool PreferUserPanels { get; set; }

    [Column("automatic_calculation")]
    public bool AutomaticCalculation { get; set; }

    [Column("startup_dashboard")]
    public string StartupDashboard { get; set; } = string.Empty;

    [Column("display_panel_code")]
    public bool DisplayPanelCode { get; set; }

    [Column("modify_version_facts")]
    public bool ModifyVersionFacts { get; set; }

    [Column("dimension_view_display_type")]
    public UserSettingsDimensionViewDisplayType DimensionViewDisplayType { get; set; }

    [Column("skin")]
    public UserSettingsSkinType Skin { get; set; } = UserSettingsSkinType.light;
    [Column("use_current_date")]
    public UseCurrentDate UseCurrentDate { get; set; } = new UseCurrentDate();

    [Column("dimension_presets")]
    public List<DimensionPreset> DimensionPresets { get; set; } = [];
}
