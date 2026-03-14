using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dog_App.Models;
namespace DOG_MANAGEMENT_APP.Controllers;

public class DogController : Controller
{
    private static List<Dog> dogs = new List<Dog>();
    private readonly IWebHostEnvironment _environment;

    public DogController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    // GET: DogController
    // GET: DogController/Details/5
    public ActionResult Details(int id)
    {
        return View(dogs.FirstOrDefault(x => x.ID == id));
    }

    // GET: DogController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: DogController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Dog d, IFormFile imagefile)
    {
        Guid rr = Guid.NewGuid();
        if (ModelState.IsValid)
        {
            if (imagefile != null && imagefile.Length > 0)
            {
                var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imagefile.FileName);
                var path = Path.Combine(_environment.WebRootPath, "images", imageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    imagefile.CopyTo(stream);
                }

                d.ImagePath = "/images/" + imageName;
            }

            dogs.Add(d);
            return RedirectToAction("Index");
        }

        return View(d);
    }

    // GET: DogController/Edit/5
    public ActionResult Edit(int id)
    {
        return View(dogs.FirstOrDefault(x => x.ID == id));
    }

    // POST: DogController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Dog dog, IFormFile? imagefile)
    {
        if (!ModelState.IsValid)
            return View(dog);

        var existing = dogs.FirstOrDefault(x => x.ID == dog.ID);
        if (existing == null) return NotFound();

        existing.Name = dog.Name;
        existing.Age = dog.Age;
        existing.Description = dog.Description;

        if (imagefile != null && imagefile.Length > 0)
        {
            var imageName = Guid.NewGuid().ToString() + Path.GetExtension(imagefile.FileName);
            var path = Path.Combine(_environment.WebRootPath, "images", imageName);

            using (var stream = new FileStream(path, FileMode.Create))
                imagefile.CopyTo(stream);

            existing.ImagePath = "/images/" + imageName;
        }
        else
        {
            // No new file — keep the old image
            existing.ImagePath = dog.ImagePath;
        }

        return RedirectToAction("Index");
    }

    // GET: DogController/Delete/5
    public ActionResult Delete(int id)
    {
        return View(dogs.FirstOrDefault(x => x.ID == id));
    }

    // POST: DogController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult Delete(Dog _dog)
    {
        try
        {
            var dog = dogs.FirstOrDefault(x => x.ID == _dog.ID);

            if (dog != null)
            {
                dogs.Remove(dog);
            }

            return RedirectToAction("Index");
        }
        catch
        {
            return View(_dog);
        }
    }
    
    public IActionResult Index(string? query)
    {
        var filtered = string.IsNullOrEmpty(query) ? 
            dogs : dogs.Where(x => x.Name!.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            
        return View(filtered);
    }
}