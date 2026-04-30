using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BorrowController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                return NotFound("Book not found.");

            if (book.AvailableCopies <= 0)
                return BadRequest("Book is not available.");

            var member = await _context.Members.FindAsync(memberId);

            if (member == null)
                return NotFound("Member not found.");

            book.AvailableCopies--;

            var record = new BorrowRecord
            {
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.Now,
                Status = "Borrowed"
            };

            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();

            return Ok("Book borrowed successfully.");
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            var record = await _context.BorrowRecords.FindAsync(borrowId);

            if (record == null)
                return NotFound("Borrow record not found.");

            if (record.Status == "Returned")
                return BadRequest("Book already returned.");

            var book = await _context.Books.FindAsync(record.BookId);

            if (book == null)
                return NotFound("Book not found.");

            record.ReturnDate = DateTime.Now;
            record.Status = "Returned";
            book.AvailableCopies++;

            await _context.SaveChangesAsync();

            return Ok("Book returned successfully.");
        }

        [HttpGet("history")]
        public async Task<IActionResult> BorrowHistory()
        {
            var history = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .ToListAsync();

            return Ok(history);
        }
    }
}
