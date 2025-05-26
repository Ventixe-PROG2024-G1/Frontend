using Frontend.Models.Image;
using System.Net.Http.Headers;

namespace Frontend.Services;

public interface IImageApiService
{
    Task<ImageResponseModel?> GetImageMetaDataAsync(Guid imageId);
    Task<ImageResponseModel?> UploadImageAsync(IFormFile imageFile);
}

public class ImageApiService(HttpClient httpClient) : IImageApiService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ImageResponseModel?> GetImageMetaDataAsync(Guid imageId)
    {
        if (imageId == Guid.Empty)
            return null;

        var response = await _httpClient.GetAsync($"api/images/{imageId}");

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<ImageResponseModel>();

        else
            return null;
    }

    public async Task<ImageResponseModel?> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(imageFile.OpenReadStream());
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
        content.Add(streamContent, "file", imageFile.FileName);

        var response = await _httpClient.PostAsync("api/images", content);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<ImageResponseModel>();
        else
            return null;
    }
}