using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace MRB.WebApiTest;

public class MyRecipesClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory factory;
    private readonly HttpClient _httpClient;

    public MyRecipesClassFixture(CustomWebApplicationFactory factory)
    {
        this.factory = factory;
        _httpClient = factory.CreateClient();
    }

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string token = null,
        string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string method, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.GetAsync(method);
    }

    protected async Task<HttpResponseMessage> DoPut(string method, object request, string token,
        string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);

        return await _httpClient.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string method, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(token);
        return await _httpClient.DeleteAsync(method);
    }

    private void ChangeRequestCulture(string culture)
    {
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}