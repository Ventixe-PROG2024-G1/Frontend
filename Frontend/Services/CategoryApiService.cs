namespace Frontend.Services;

public class CategoryApiService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable>
}
