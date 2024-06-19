using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class DishType : Entity
{
    public Enums.DishType Type { get; set; }
    public long RecipeId { get; set; }
}