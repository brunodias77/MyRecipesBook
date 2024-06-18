using System.Net;
using Xunit;
using FluentAssertions;

namespace MRB.WebApiTest.Users.GetAll;

public class GetAllUsersIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    public GetAllUsersIntegrationTest(CustomWebApplicationFactory factory)
    {
        httpClient = factory.CreateClient();
    }

    private readonly HttpClient httpClient;
    private readonly string method = "User";

    [Fact]
    public async Task SUCESSO()
    {
        var response = await httpClient.GetAsync(method);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}