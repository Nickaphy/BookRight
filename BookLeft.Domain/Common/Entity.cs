// Base class for all entities
// Provides identity (Id)
// Used to distinguish entities from value objects
namespace BookRight.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; private set; }
    
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}