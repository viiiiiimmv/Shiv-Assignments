using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dog_App.Models;
namespace DOG_MANAGEMENT_APP.Controllers
{
    public class DogController : Controller
    {
        private static List<Dog> dogs = new List<Dog>();
        private readonly IWebHostEnvironment _environment;

        public DogController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        // GET: DogController
        public ActionResult Index()
        {
            return View(dogs);
        }

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
        public ActionResult Edit(Dog dog)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dogs.FirstOrDefault(x => x.ID == dog.ID));
                }
                else
                {
                    foreach (var _dog in dogs)
                    {
                        if (_dog.ID == dog.ID)
                        {
                            _dog.Name = dog.Name;
                            _dog.ImagePath = dog.ImagePath;
                            _dog.Description = dog.Description;
                            _dog.Age = dog.Age;
                        }
                    }
                    
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
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
    }
}
