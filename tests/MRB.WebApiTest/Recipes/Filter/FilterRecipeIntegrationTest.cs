using System.Globalization;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Requests.Recipes;
using MRB.CommonTest.Tokens;
using MRB.Communication.Requests.Recipes.Filter;
using MRB.Domain.Enums;
using MRB.Exceptions;
using MRB.Infra.Security.Tokens.Generator;
using MRB.WebApiTest.InlineData;
using Xunit;

namespace MRB.WebApiTest.Recipes.Filter;

public class FilterRecipeIntegrationTest : MyRecipesClassFixture
{
    private const string METHOD = "recipe/filter";
    private readonly Guid _userId;
    private string _recipeTitle;
    private Difficulty _recipeDifficultyLevel;
    private CookingTime _recipeCookingTime;
    private DishType _recipeDishType;


    public FilterRecipeIntegrationTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SUCESSO()
    {
    }

    [Fact]
    public async Task SUCESSO_SEM_CONTEUDO()
    {
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ERRO_TEMPO_DE_COZIMENTO_INVALIDO(string culture)
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((MRB.Domain.Enums.CookingTime)1000);
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        var response = await DoPost(METHOD, request: request, token: token, culture: culture);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var erros = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage =
            ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));
        erros.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}