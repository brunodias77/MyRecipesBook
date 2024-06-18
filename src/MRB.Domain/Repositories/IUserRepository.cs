using MRB.Domain.Entities;

namespace MRB.Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    
    Task AddAsync(User user);

}