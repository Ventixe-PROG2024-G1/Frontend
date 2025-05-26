using Frontend.Models.Event.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Services;

public interface ICategoryApiService
{
    Task<IEnumerable<SelectListItem>> GetCategoriesAsSelectListItemsAsync(string allCategoriesText = "All Category", string allCategoriesValue = "");
    Task<IEnumerable<CategoryResponseModel>> GetEventCategoriesAsync();
}

public class CategoryApiService(HttpClient httpClient) : ICategoryApiService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<CategoryResponseModel>> GetEventCategoriesAsync()
    {
        var categories = await _httpClient.GetFromJsonAsync<List<CategoryResponseModel>>("api/category");

        return categories ?? new List<CategoryResponseModel>();
    }

    public async Task<IEnumerable<SelectListItem>> GetCategoriesAsSelectListItemsAsync(string allCategoriesText = "All Category", string allCategoriesValue = "")
    {
        var categories = await GetEventCategoriesAsync();
        var selectedListItems = new List<SelectListItem>();

        if (!string.IsNullOrEmpty(allCategoriesText))
        {
            selectedListItems.Add(new SelectListItem
            {
                Value = allCategoriesValue,
                Text = allCategoriesText
            });
        }

        var categoryOptions = categories
            .Where(c => c.CategoryName != null)
            .Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName ?? "Unknown Category"
            })
            .OrderBy(item => item.Text)
            .ToList();

        selectedListItems.AddRange(categoryOptions);
        return selectedListItems;
    }
}
