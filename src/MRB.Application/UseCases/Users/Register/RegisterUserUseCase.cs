using MRB.Communication.Requests.Users;
using MRB.Communication.Responses.Users;
using MRB.Domain.Repositories;
using MRB.Domain.Security;
using MRB.Domain.Security.Token;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        await _unitOfWork.BeginTransactionAsync();
        
        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();
        await _unitOfWork.CommitAsync();
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);
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