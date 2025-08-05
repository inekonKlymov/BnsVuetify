namespace Bns.Domain.Abstracts;

public interface IStrongTypeId<TId> where TId : notnull
{
    TId Value { get; } 


}