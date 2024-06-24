using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;
using MRB.Domain.Enums;

namespace MRB.Domain.Entities;

public class Recipe : Entity
{
    public string Title { get; set; } = String.Empty;
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }

    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();
    public ICollection<DishType> DishTypes { get; set; } = new List<DishType>();

    // Chave estrangeira para o usuário
    public Guid UserId { get; set; }

    public Recipe()
    {
    }
}