using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class Instruction : Entity
{
    [Required] public int Step { get; set; }

    [Required] [MaxLength(2000)] public string Text { get; set; }

    [Required] public Guid RecipeId { get; set; }

    [ForeignKey("RecipeId")] public Recipe Recipe { get; set; }

    public Instruction()
    {
    }
    // public int Step { get; set; }
    // public string Text { get; set; }
    //
    // // Chave estrangeira para a receita
    // public Guid RecipeId { get; set; }
    // public Recipe Recipe { get; set; }
    //
    // public Instruction()
    // {
    // }
}