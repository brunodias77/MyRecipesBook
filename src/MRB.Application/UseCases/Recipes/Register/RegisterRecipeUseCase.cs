using AutoMapper;
using MRB.Communication.Requests.Recipes.Register;
using MRB.Communication.Responses.Recipes;
using MRB.Domain.Entities;
using MRB.Domain.Repositories;
using MRB.Domain.Services;
using MRB.Exceptions.Exceptions;

namespace MRB.Application.UseCases.Recipes.Register;

public class RegisterRecipeUseCase : IRegisterRecipeUseCase
{
    public RegisterRecipeUseCase(IRecipeRepository recipeRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _recipeRepository = recipeRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private readonly IRecipeRepository _recipeRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeJson request)
    {
        Validate(request);
        var loggedUser = _loggedUser.User();
        var recipe = _mapper.Map<Recipe>(request);
        recipe.UserId = loggedUser.Result.Id;
        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var i = 0; i < instructions.Count; i++)
        {
            instructions.ElementAt(i).Step = i + 1;
        }

        recipe.Instructions = _mapper.Map<IList<Instruction>>(instructions);
        
        await _recipeRepository.Add(recipe);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
    }

    private static void Validate(RequestRegisterRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);
        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
        }
    }
}