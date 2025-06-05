using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Feed.Areas.Feed.Database;
using WebApp_Feed.Areas.Feed.Models;
public class AddBreedModel : PageModel
{
    private readonly GreenswampContext _context;

    public AddBreedModel(GreenswampContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var name = Request.Form["Name"];
        var description = Request.Form["Description"];
        var imageUrl = Request.Form["ImageUrl"];
        var temperamentInput = Request.Form["Temperament"]; // например: "Active, Gentle, Loyal"

        var newBreed = new Breed
        {
            Name = name,
            Description = description,
            Images = new List<Image>
            {
                new Image { Url = imageUrl }
            },
            Temperaments = new List<Temperament>()
        };


        // Добавляем изображение
        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            newBreed.Images.Add(new Image
            {
                Url = imageUrl
            });
        }

        // Добавляем темпераменты
        if (!string.IsNullOrWhiteSpace(temperamentInput))
        {
            var temps = temperamentInput.ToString()
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim());

            foreach (var temp in temps)
            {
                newBreed.Temperaments.Add(new Temperament
                {
                    Name = temp
                });
            }
        }

        _context.Breeds.Add(newBreed);
        await _context.SaveChangesAsync();


        // Выводим только добавленную породу и её связи
        Console.WriteLine("=== Новая порода добавлена ===");
        Console.WriteLine($"Breed ID: {newBreed.Id}");
        Console.WriteLine($"Name: {newBreed.Name}");
        Console.WriteLine($"Description: {newBreed.Description}");
        Console.WriteLine($"ApiId: {newBreed.ApiId}");

        if (newBreed.Images != null)
        {
            Console.WriteLine("Images:");
            foreach (var img in newBreed.Images)
            {
                Console.WriteLine($"  - Image ID: {img.Id}, URL: {img.Url}, BreedId: {img.BreedId}");
            }
        }

        if (newBreed.Temperaments != null)
        {
            Console.WriteLine("Temperaments:");
            foreach (var temp in newBreed.Temperaments)
            {
                Console.WriteLine($"  - Temperament ID: {temp.Id}, Name: {temp.Name}, BreedId: {temp.BreedId}");
            }
        }


        return RedirectToPage("/Index");
    }
}
