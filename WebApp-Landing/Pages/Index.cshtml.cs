using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Landing.ApiModels;
using System.Text.Json;

namespace WebApp_Landing.Pages
{
public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;

    public List<(string Name, string Description, string ImageUrl)> Breeds { get; set; }

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task OnGetAsync()
    {
        var breedResponse = await _httpClient.GetAsync("https://api.thecatapi.com/v1/breeds");
        if (!breedResponse.IsSuccessStatusCode) return;

        var breedJson = await breedResponse.Content.ReadAsStringAsync();
        var breeds = JsonSerializer.Deserialize<List<BreedApiModel>>(breedJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Breeds = new List<(string, string, string)>();

        foreach (var breed in breeds)
        {
            string imageUrl = "";
            if (!string.IsNullOrEmpty(breed.Reference_Image_Id))
            {
                var imageResponse = await _httpClient.GetAsync($"https://api.thecatapi.com/v1/images/{breed.Reference_Image_Id}");
                    if (imageResponse.IsSuccessStatusCode)
                    {
                        var imageJson = await imageResponse.Content.ReadAsStringAsync();
                        var image = JsonSerializer.Deserialize<BreedImageModel>(imageJson,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        imageUrl = image?.Url ?? "";
                    Console.WriteLine($"Breed: {breed.Name}, Image: {imageUrl}");

                }
            }

            Breeds.Add((breed.Name, breed.Description, imageUrl));
        }
    }
}

}