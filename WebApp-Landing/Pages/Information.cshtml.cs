using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp_Feed.Areas.Feed.Database;
using WebApp_Feed.Areas.Feed.Models;

public class InformationModel : PageModel
{
    private readonly GreenswampContext _context;

    public InformationModel(GreenswampContext context)
    {
        _context = context;
    }

    public Breed Breed { get; set; }

     public async Task<IActionResult> OnGetAsync(int id)
    {
        Breed = await _context.Breeds
            .Include(b => b.Images)
            .Include(b => b.Temperaments)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (Breed == null)
            return NotFound();

        return Page();
    }
}
