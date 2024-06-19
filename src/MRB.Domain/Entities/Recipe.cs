using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;
using MRB.Domain.Enums;

namespace MRB.Domain.Entities;

public class Recipe : Entity
{
    public string Title { get; set; } = String.Empty;
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public IList<Ingredient> Ingredients { get; set; } = [];
    public IList<Instruction> Instructions { get; set; } = [];
    [NotMapped] public IList<DishType> DishTypes { get; set; } = [];
    public Guid UserId { get; set; }

    public Recipe()
    {
    }
}