using CRUD_OPERATIONS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_OPERATIONS.Controllers
{
    public class DogController : Controller
    {
        private readonly AppDbContext _context;

        public DogController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DogController
        public async Task<IActionResult> Index()
        {
            var dogs = await _context.Dogs
                .AsNoTracking()
                .OrderBy(dog => dog.ID)
                .ToListAsync();

            return View(dogs);
        }

        // GET: DogController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var dog = await _context.Dogs
                .AsNoTracking()
                .FirstOrDefaultAsync(existingDog => existingDog.ID == id);

            if (dog is null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: DogController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", dog);
            }

            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: DogController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dog = await _context.Dogs.FindAsync(id);

            if (dog is null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Dog dog)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", dog);
            }

            var existingDog = await _context.Dogs.FindAsync(dog.ID);

            if (existingDog is null)
            {
                return NotFound();
            }

            existingDog.Name = dog.Name;
            existingDog.Age = dog.Age;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: DogController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var dog = await _context.Dogs
                .AsNoTracking()
                .FirstOrDefaultAsync(existingDog => existingDog.ID == id);

            if (dog is null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Dog dog)
        {
            var existingDog = await _context.Dogs.FindAsync(dog.ID);

            if (existingDog is null)
            {
                return NotFound();
            }

            _context.Dogs.Remove(existingDog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
