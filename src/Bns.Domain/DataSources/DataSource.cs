using Bns.Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bns.Domain.DataSources;

public readonly struct DataSourceId(Guid value) : IStrongTypeId<Guid>
{
    public Guid Value { get; } = value;

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(DataSourceId id) => id.Value;

    public static explicit operator DataSourceId(Guid value) => new(value);
}

[Table("data_source")]
public class DataSource : Entity<DataSourceId>
{
    [Column("name")]
    [MaxLength(30)]
    public required string Name { get; set; }

    [Column("type")]
    public required DataSourceTypeEnum Type { get; set; }

    [Column("enabled")]
    public required bool Enabled { get; set; }

    [Column("connection_string")]
    public required string ConnectionString { get; set; }

    [Column("create_date")]
    public required DateTime CreateDate { get; set; }

    [Column("modify_date")]
    public DateTime ModifyDate { get; set; }

    public DataSourceOlapConfig? Configuration { get; set; }

    [ForeignKey(nameof(Configuration))]
    [Column("ConfigurationId")]
    public DataSourceOlapConfigId? ConfigurationId { get; set; }
}

public class DataSourceEntityConfiguration : IEntityTypeConfiguration<DataSource>
{
    public void Configure(EntityTypeBuilder<DataSource> builder)
    {
        builder.Property(x => x.Type).HasConversion<string>();
    }
}

public enum DataSourceTypeEnum
{
    MSSQL,
    MSOLAP
}