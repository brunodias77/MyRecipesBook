using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class DishType : Entity
{
    public DishType Type { get; set; }

    // Chave estrangeira para a receita
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; }

    public DishType()
    {
    }
}