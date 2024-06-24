using Microsoft.EntityFrameworkCore;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.Infra.Data.Repositories;

public class RecipeRepository : IRecipeRepository
{
    public RecipeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly ApplicationDbContext _context;

    public async Task Add(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task<IList<Recipe>> GetAll()
    {
        return await _context.Recipes
            .Include(r => r.Instructions)
            .Include(r => r.Ingredients)
            .Include(r => r.DishTypes)
            .ToListAsync();
    }
}