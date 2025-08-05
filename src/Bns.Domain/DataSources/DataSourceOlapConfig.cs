using Bns.Domain.Abstracts;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bns.Domain.DataSources;

public readonly struct DataSourceOlapConfigId(Guid value) : IStrongTypeId<Guid>
{
    public Guid Value { get; } = value;

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(DataSourceOlapConfigId id) => id.Value;

    public static explicit operator DataSourceOlapConfigId(Guid value) => new(value);
}

[Table("data_source_olap_config")]
public class DataSourceOlapConfig : Entity<DataSourceOlapConfigId>
{
    [DefaultValue(true)]
    [Column("impersonate")]
    public bool Impersonate { get; set; } = true;

    [DefaultValue(0)]
    [Column("load_members_count")]
    public int LoadMembersCount { get; set; } = 0;

    [DefaultValue(0)]
    [Column("fiscal_offset")]
    public int FiscalOffset { get; set; } = 0;

    [DefaultValue(false)]
    [Column("use_fiscal_offset")]
    public bool PoolingEnabled { get; set; } = false;

    [DefaultValue(10)]
    [Column("pool_size")]
    public int PoolSize { get; set; } = 10;

    [DefaultValue(false)]
    [Column("load_member_code_properties")]
    public bool LoadMemberCodeProperties { get; set; } = false;

    [DefaultValue(true)]
    [Column("use_olap_notes")]
    public bool UseOlapNotes { get; set; } = true;

    [DefaultValue(true)]
    [Column("allow_writeback_on_aggregation_level")]
    public bool AllowWritebackOnAggregationLevel { get; set; } = true;

    [DefaultValue(1000)]
    [Column("writeback_leaf_limit")]
    public int WritebackLeafLimit { get; set; } = 1000;
}