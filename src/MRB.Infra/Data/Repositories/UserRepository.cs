using Microsoft.EntityFrameworkCore;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly ApplicationDbContext _context;

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}