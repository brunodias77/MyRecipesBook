using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

[Table("Instructions")]
public class Ingredient : Entity
{
    public string Item { get; set; } = string.Empty;
    public Guid RecipeId { get; set; }
}