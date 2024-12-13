using System.Net.Http.Json;

namespace BlazorMudLab.Services;

public class Api
{
    private readonly IHttpClientFactory _httpClient;
    private readonly HttpClient _http;

    public Api(IHttpClientFactory httpClient) {
        _httpClient = httpClient;
        _http = _httpClient.CreateClient("Api1");
    }

    public async Task<T> ApiGet<T>(string api) {
        return await _http.GetFromJsonAsync<T>($"{api}");
    }

    public async Task<HttpResponseMessage> ApiPost(string api, object param) {
        var response = await _http.PostAsJsonAsync($"{api}", param);
        return response;
    }

    public async Task<HttpResponseMessage> ApiPut(string api, object param) {
        var response = await _http.PutAsJsonAsync($"{api}", param);
        return response;
    }

    public async Task<HttpResponseMessage> ApiDelete(string api) {
        var response = await _http.DeleteAsync($"{api}");
        return response;
    }
}