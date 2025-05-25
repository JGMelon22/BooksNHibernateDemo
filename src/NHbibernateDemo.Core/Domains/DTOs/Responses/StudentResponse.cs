namespace NHbibernateDemo.Core.Domains.DTOs.Responses;

public record StudentResponse
{
    public virtual int Id { get; init; }
    public virtual string Name { get; init; } = string.Empty;
    public virtual string Course { get; init; } = string.Empty;
    public virtual string Gender { get; init; } = string.Empty;
}