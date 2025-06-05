using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApp_Feed.Areas.Feed.Database;
using WebApp_Feed.Areas.Feed.Models;
using WebApp_Feed.Areas.Feed.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;

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
                    Images = new List<Image>(),
                    Temperaments = new List<Temperament>()
                };

                if (!string.IsNullOrEmpty(breed.Reference_Image_Id))
                {
                    var imageJson = await http.GetStringAsync($"https://api.thecatapi.com/v1/images/{breed.Reference_Image_Id}");
                    await Task.Delay(500); // пауза 200 мс между запросами

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

                if (!string.IsNullOrEmpty(breed.Temperament))
                {
                    var temperaments = breed.Temperament.Split(", ", StringSplitOptions.RemoveEmptyEntries);

                    foreach (var temp in temperaments)
                    {
                        newBreed.Temperaments.Add(new Temperament { Name = temp });
                    }
                }


                _context.Breeds.Add(newBreed);
            }

            await _context.SaveChangesAsync();
            
        }

        Breeds = await _context.Breeds
                .Include(b => b.Images)
                .Include(b => b.Temperaments)
                .ToListAsync();
                
        
        // Console.WriteLine("=== Таблица Breeds ===");
        // foreach (var breed in Breeds)
        // {
        //     Console.WriteLine($"ID: {breed.Id}, Name: {breed.Name}, ApiId: {breed.ApiId}, Description: {breed.Description}");

        //     if (breed.Images?.Any() == true)
        //     {
        //         foreach (var image in breed.Images)
        //         {
        //             Console.WriteLine($"   └── Image ID: {image.Id}, URL: {image.Url}, RefId: {image.ReferenceImageId}");
        //         }
        //     }

        //     if (breed.Temperaments?.Any() == true)
        //     {
        //         foreach (var temp in breed.Temperaments)
        //         {
        //             Console.WriteLine($"   └── Temperament ID: {temp.Id}, Name: {temp.Name}");
        //         }
        //     }
        // }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var breed = await _context.Breeds
            .Include(b => b.Images)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (breed != null)
        {
            _context.Breeds.Remove(breed);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

}