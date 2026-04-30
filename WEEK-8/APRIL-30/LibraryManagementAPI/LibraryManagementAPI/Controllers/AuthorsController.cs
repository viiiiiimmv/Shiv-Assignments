using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();
        }

        [HttpGet("id")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include (a => a.Books)
                .FirstOrDefaultAsync(p => p.AuthorId  == id);

            if ( author == null) return NotFound();

            return author;
        }

        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(Author author) 
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, author);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateAuthor(int id,  Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var book = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);

            if (book == null) return NotFound();

            _context.Authors.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
