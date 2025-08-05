using Bns.Domain.Common.Errors;

namespace Bns.Domain.DataSources.Errors;

public static class DataSourceDomainErrors
{
    public static readonly NotFoundError DataSourceNotFound = new("Data source not found.");
    public static readonly DomainError DataSourceAlreadyExists = new("Data source already exists.");
    public static readonly DomainError DataSourceInvalidConfiguration = new("Data source has invalid configuration.");
    public static readonly DomainError DataSourceConnectionFailed = new("Failed to connect to the data source.");
    public static readonly DomainError DataSourceOperationFailed = new("Data source operation failed.");
}