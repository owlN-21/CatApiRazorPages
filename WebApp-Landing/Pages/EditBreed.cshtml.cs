using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp_Feed.Areas.Feed.Database;
using WebApp_Feed.Areas.Feed.Models;

public class EditBreedModel : PageModel
{
    private readonly GreenswampContext _context;

    public EditBreedModel(GreenswampContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Breed Breed { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Breed = await _context.Breeds.FindAsync(id);
        if (Breed == null)
        {
            return NotFound();
        }

        return Page();
    }

   public async Task<IActionResult> OnPostAsync()
    {
        System.IO.File.AppendAllText("log.txt", $"OnPostAsync вызван в {DateTime.Now}\n");

        // if (!ModelState.IsValid)
        // {
        //     return Page();
        // }

        var breedInDb = await _context.Breeds.FindAsync(Breed.Id);
        if (breedInDb == null)
        {
            return NotFound();
        }

        // Обновляем только нужные поля
        breedInDb.Name = Breed.Name;
        breedInDb.Description = Breed.Description;

        Console.WriteLine($"Изменяем породу: ID = {Breed.Id}, Name = {Breed.Name}, Description = {Breed.Description}");


        await _context.SaveChangesAsync();


        return RedirectToPage("/Information", new { area = "Landing", id = Breed.Id });

    }


}
