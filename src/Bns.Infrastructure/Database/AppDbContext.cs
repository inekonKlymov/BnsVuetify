using Bns.Domain.Abstracts;
using Bns.Domain.DataSources;
using Bns.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Bns.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<DataSource> DataSources { get; set; }
    public DbSet<DataSourceOlapConfig> DataSourceOlapConfigs { get; set; }
    public DbSet<Language> Languages { get; set; }
    //public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Domain.DependencyInjection).Assembly);
        AddStrongTypeIdConversion(modelBuilder);
    }
    private void AddStrongTypeIdConversion(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var prop in entityType.ClrType.GetProperties())
            {
                var propType = prop.PropertyType;
                var strongTypeId = propType.GetInterfaces().FirstOrDefault(s => s.IsGenericType && s.GetGenericTypeDefinition() == typeof(IStrongTypeId<>));
                if (strongTypeId is null) continue;
                var valueProp = propType.GetProperty("Value");
                if (valueProp == null) continue;

                Type valueType = valueProp.PropertyType;
                ValueConverter? converter = valueType switch
                {
                    Type t when t == typeof(int) => CreateConverter<int>(propType),
                    Type t when t == typeof(Guid) => CreateConverter<Guid>(propType),
                    Type t when t == typeof(long) => CreateConverter<long>(propType),
                    Type t when t == typeof(string) => CreateConverter<string>(propType),
                    _ => null
                };

                if (converter == null) continue;

                modelBuilder.Entity(entityType.ClrType)
                    .Property(prop.Name)
                    .HasConversion(converter);
            }
        }
    }
    private static ValueConverter CreateConverter<TValue>(Type idType)
    {
        // id => id.Value
        var idParam = Expression.Parameter(idType, "id");
        var toValue = Expression.Lambda(
            Expression.Property(idParam, "Value"),
            idParam
        );

        // value => new IdType(value)
        var valueParam = Expression.Parameter(typeof(TValue), "value");
        var ctor = idType.GetConstructor(new[] { typeof(TValue) });
        if (ctor == null)
            throw new InvalidOperationException($"No constructor found on {idType} with parameter of type {typeof(TValue)}");

        var fromValue = Expression.Lambda(
            Expression.New(ctor, valueParam),
            valueParam
        );

        var converterType = typeof(ValueConverter<,>).MakeGenericType(idType, typeof(TValue));
        return (ValueConverter)Activator.CreateInstance(
            converterType,
            toValue,
            fromValue,
            null
        )!;
    }

}