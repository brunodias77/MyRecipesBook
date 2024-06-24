using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class Ingredient : Entity
{
    public string Item { get; set; } = string.Empty;

    // Chave estrangeira para a receita
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; }

    public Ingredient()
    {
    }
}