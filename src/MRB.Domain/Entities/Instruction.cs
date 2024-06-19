using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class Instruction : Entity
{
    public int Step { get; set; }
    public string Text { get; set; }
    public Guid RecipeId { get; set; }
}