using FluentAssertions;
using MRB.Application.UseCases.Recipes;
using MRB.Application.UseCases.Recipes.Register;
using MRB.CommonTest.Entities;
using MRB.CommonTest.LoggedUser;
using MRB.CommonTest.Mapper;
using MRB.CommonTest.Repositories;
using MRB.CommonTest.Requests;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.UnitOfWork;
using MRB.Domain.Entities;
using MRB.Exceptions;
using MRB.Exceptions.Exceptions;
using Xunit;

namespace MRB.UseCaseTest.Recipes.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task SUCESSO()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRecipeJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute(request);
        result.Should().NotBeNull();
        // result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task ERRO_TITULO_VAZIO()
    {
        // Arrange
        var (user, _) = UserBuilder.Build();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = string.Empty; // Título vazio para disparar a validação
        var useCase = CreateUseCase(user);

        // Act
        Func<Task> act = async () => { await useCase.Execute(request); };

        // Assert
        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.ErrorMessages.Should().ContainSingle()
            .Which.Should().Be(ResourceMessagesException.RECIPE_TITLE_EMPTY);
    }

    [Fact]
    public async Task ERRO_INSTRUCOES_MUITO_LONGAS()
    {
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions.First().Text = RequestStringGenerator.Paragraphs(2001);
        var validator = new RecipeValidator();
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e =>
            e.ErrorMessage.Equals(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
    }

    private static RegisterRecipeUseCase CreateUseCase(User user)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = RecipeRepositoryBuilder.Build();
        return new RegisterRecipeUseCase(repository, loggedUser, unitOfWork, mapper);
    }
}