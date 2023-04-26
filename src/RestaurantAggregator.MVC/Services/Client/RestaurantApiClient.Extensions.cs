using System.Net.Http.Headers;

namespace RestaurantAggregator.MVC.Services.Client;

public partial class RestaurantApiClient
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

public partial interface IRestaurantApiClient
{
    void SetToken(string token);
}