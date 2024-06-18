using Microsoft.AspNetCore.Mvc;
using MRB.Communication.Requests.Users;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;

namespace MRB.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private readonly IUserRepository _userRepository;

    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
        var users = await _userRepository.GetAll();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RequestRegisterUserJson request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };

        await _userRepository.AddAsync(user);
        return Ok();
    }
}