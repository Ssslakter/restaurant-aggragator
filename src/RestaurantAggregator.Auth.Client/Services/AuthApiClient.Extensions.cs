using System.Net.Http.Headers;

namespace RestaurantAggregator.Auth.Client.Services;

public partial class AuthApiClient
{
    private string _token;
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
    {
        if (!string.IsNullOrEmpty(_token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
    }

    public void SetToken(string token)
    {
        _token = token;
    }
}

public partial interface IAuthApiClient
{
    void SetToken(string token);
}