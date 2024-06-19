using Microsoft.AspNetCore.Mvc;
using MRB.Api.Attributes;
using MRB.Application.UseCases.Users.Register;
using MRB.Communication.Requests.Users;
using MRB.Communication.Responses.Users;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;
using MRB.Domain.Security;

namespace MRB.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController(IUserRepository userRepository, IPasswordEncripter passwordEncripter)
    {
        _userRepository = userRepository;
        _passwordEncripter = passwordEncripter;
    }

    private readonly IUserRepository _userRepository;
    private readonly IPasswordEncripter _passwordEncripter;

    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
        var users = await _userRepository.GetAll();
        return Ok(users);
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    // [HttpPost]
    // public async Task<IActionResult> Register(RequestRegisterUserJson request)
    // {
    //     var user = new User
    //     {
    //         Name = request.Name,
    //         Email = request.Email,
    //         Password = _passwordEncripter.Encrypt(request.Password)
    //     };
    //
    //     await _userRepository.AddAsync(user);
    //     return Ok();
    // }
}