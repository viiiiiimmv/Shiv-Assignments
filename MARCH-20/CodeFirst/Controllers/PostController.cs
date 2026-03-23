using CodeFirst.Data;
using CodeFirst.Models;
using CodeFirst.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Controllers;

public class PostController : Controller
{
    private readonly EventContext _context;
        public PostController(EventContext context)
        {
            _context = context;
        }
        
        // GET: PostController
        public async Task<ActionResult> Index()
        {
            var posts=await _context.Posts.ToListAsync();
            return View(posts);
        }

        // GET: PostController/Details/5
        public async Task<ActionResult> Details(int id)
        {
         var post=await _context.GetPostByID(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
             _postRepository.InsertPost(post);
                _postRepository.Save();
            return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            var post = _postRepository.GetPostByID(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,Post post)
        {
          if(id!=post.Id)
            {
                return NotFound();
            }
         else
            {
                _postRepository.UpdatePost(post);
                _postRepository.Save();
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            var post = _postRepository.GetPostByID(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: PostController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
       public ActionResult DeleteConfirmed(int id)
        {
            _postRepository.DeletePost(id);
            _postRepository.Save();
            return RedirectToAction(nameof(Index));
        }
}