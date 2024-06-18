namespace MRB.Domain.Abstractions;

public class Entity
{
    public Guid Id { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
}