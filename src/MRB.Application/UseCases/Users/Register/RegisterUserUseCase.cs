using AutoMapper;
using FluentValidation;
using MRB.Communication.Requests.Users;
using MRB.Communication.Responses;
using MRB.Communication.Responses.Users;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;
using MRB.Domain.Security;
using MRB.Domain.Security.Token;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    public RegisterUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
        _mapper = mapper;
    }

    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IMapper _mapper;

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        var token = _accessTokenGenerator.Generate(user.Id);
        await _userRepository.AddAsync(user);
        var response = new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokenJson
            {
                AccessToken = token
            }
        };
        return response;
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);
        var emailExists = await _userRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExists)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty,
                ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}