using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using WebApp_Feed.Areas.Feed.Database;
using WebApp_Feed.Areas.Feed.Models;
using WebApp_Feed.Areas.Feed.Models.ApiModels;

namespace WebApp_Feed.Areas.Feed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly GreenswampContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public SeedController(GreenswampContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("import")]
        public async Task<IActionResult> ImportBreedsFromApi()
        {
            var http = _httpClientFactory.CreateClient();
            var response = await http.GetAsync("https://api.thecatapi.com/v1/breeds");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Ошибка при запросе к внешнему API.");

            var json = await response.Content.ReadAsStringAsync();
            var breeds = JsonSerializer.Deserialize<List<BreedApiModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            foreach (var breed in breeds)
            {
                if (await _context.Breeds.AnyAsync(b => b.ApiId == breed.Id)) continue;

                var newBreed = new Breed
                {
                    ApiId = breed.Id,
                    Name = breed.Name,
                    Description = breed.Description,
                    Images = new List<Image>()
                };

                // Загрузка изображения
                if (!string.IsNullOrEmpty(breed.Reference_Image_Id))
                {
                    var imageResp = await http.GetAsync($"https://api.thecatapi.com/v1/images/{breed.Reference_Image_Id}");
                    if (imageResp.IsSuccessStatusCode)
                    {
                        var imageJson = await imageResp.Content.ReadAsStringAsync();
                        var image = JsonSerializer.Deserialize<BreedImageModel>(imageJson,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (!string.IsNullOrEmpty(image?.Url))
                        {
                            newBreed.Images.Add(new Image
                            {
                                ReferenceImageId = breed.Reference_Image_Id,
                                Url = image.Url
                            });
                        }
                    }
                }

                _context.Breeds.Add(newBreed);
                await _context.SaveChangesAsync();
            }

            return Ok("Импорт завершен успешно.");
        }

        [HttpDelete("reset")]
        public async Task<IActionResult> ResetDatabase()
        {
            _context.Images.RemoveRange(_context.Images);
            _context.Breeds.RemoveRange(_context.Breeds);
            await _context.SaveChangesAsync();
            return Ok("База данных очищена.");
        }

    }
}
