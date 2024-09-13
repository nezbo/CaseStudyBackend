using System.ComponentModel.DataAnnotations;

namespace Microservice.Common.Domain.Models;

public abstract class Entity(Guid? id) : IIdentity
{
    [Key]
    public Guid Id { get; set; } = id ?? Guid.Empty;

    public override bool Equals(object? obj)
    {
        if(obj is null || obj.GetType() != typeof(Entity))
        {
            return false;
        }

        return ((Entity)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected Entity() : this(Guid.NewGuid()) { }
}
