using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp_Feed.Areas.Feed.Models;
using WebApp_Feed.Areas.Feed.Database;
using Microsoft.EntityFrameworkCore;


namespace WebApp_Feed.Areas.Feed.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly GreenswampContext _context;

        public BreedsController(GreenswampContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Breed>>> GetBreeds()
        {
            return await _context.Breeds.Include(b => b.Images).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Breed>> GetBreed(int id)
        {
            var breed = await _context.Breeds
                                    .Include(b => b.Images)
                                    .Include(b => b.Comments)
                                    .FirstOrDefaultAsync(b => b.Id == id);

            if (breed == null)
                return NotFound();

            return breed;
        }
    }

}