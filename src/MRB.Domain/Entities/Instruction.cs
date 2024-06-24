using System.ComponentModel.DataAnnotations.Schema;
using MRB.Domain.Abstractions;

namespace MRB.Domain.Entities;

public class Instruction : Entity
{
    public int Step { get; set; }
    public string Text { get; set; }

    // Chave estrangeira para a receita
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; }

    public Instruction()
    {
    }
}