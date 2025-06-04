using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp_Feed.Areas.Feed.Database;
using WebApp_Feed.Areas.Feed.Models;
using WebApp_Feed.Areas.Feed.Models.ApiModels;

public class IndexModel : PageModel
{
    private readonly GreenswampContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public List<Breed> Breeds { get; set; }

    public IndexModel(GreenswampContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        if (!_context.Breeds.Any())
        {
            var http = _httpClientFactory.CreateClient();
            var json = await http.GetStringAsync("https://api.thecatapi.com/v1/breeds");

            var breeds = JsonSerializer.Deserialize<List<BreedApiModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var breed in breeds)
            {
                var newBreed = new Breed
                {
                    ApiId = breed.Id,
                    Name = breed.Name,
                    Description = breed.Description,
                    Images = new List<Image>()
                };

                if (!string.IsNullOrEmpty(breed.Reference_Image_Id))
                {
                    var imageJson = await http.GetStringAsync($"https://api.thecatapi.com/v1/images/{breed.Reference_Image_Id}");
                    var imageData = JsonSerializer.Deserialize<BreedImageModel>(imageJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (!string.IsNullOrEmpty(imageData?.Url))
                    {
                        newBreed.Images.Add(new Image
                        {
                            ReferenceImageId = breed.Reference_Image_Id,
                            Url = imageData.Url
                        });
                    }
                }

                _context.Breeds.Add(newBreed);
            }

            await _context.SaveChangesAsync();
        }

        Breeds = await _context.Breeds.Include(b => b.Images).ToListAsync();
    }
}
