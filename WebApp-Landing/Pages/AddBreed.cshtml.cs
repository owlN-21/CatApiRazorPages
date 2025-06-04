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

        var newBreed = new Breed
        {
            Name = name,
            Description = description,
            Images = new List<Image>
            {
                new Image
                {
                    Url = imageUrl
                }
            }
        };

        _context.Breeds.Add(newBreed);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Index");
    }
}
