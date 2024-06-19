using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using MRB.CommonTest.Requests.Users;
using Xunit;

namespace MRB.WebApiTest.Users.Register;

public class RegisterUserIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    public RegisterUserIntegrationTest(CustomWebApplicationFactory factory)
    {
        this.httpClient = factory.CreateClient();
    }

    private readonly HttpClient httpClient;
    private readonly string method = "user/signup";

    [Fact]
    public async Task SUCESSO()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await httpClient.PostAsJsonAsync(method, request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString()
            .Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
    }
}