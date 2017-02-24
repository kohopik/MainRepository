using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IndieProjects.Model;

namespace IndieProjects.Controllers
{
    public class HomeController : Controller
    {
        IndieContext dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public HomeController(IndieContext context,UserManager<User> userManager, SignInManager<User> signInManager)
        {
            dbContext = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public PartialViewResult Comments(int id)
        {
            List<ArticleCommentaries> comments = dbContext.ArticleCommentaries.Include(x => x.Author).Include(x => x.Article).Where(x => x.Article.ID == id).ToList();
            return PartialView(comments);
        }

        public async Task<IActionResult> AddCommentToArticle(string txt, int id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Article currentArticle = dbContext.Articles.FirstOrDefault(x => x.ID == id);
            dbContext.ArticleCommentaries.Add(new ArticleCommentaries
            {
                Content = txt,
                Author = user,
                DateSend = DateTime.Now,
                Article = currentArticle
            });
            await dbContext.SaveChangesAsync();
            return RedirectToAction("CurrentArticle/" + id.ToString(), "Home");
        }

        public async Task<IActionResult> AddSelfCommentToArticle(string txt, int idSelfComment)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ArticleCommentaries SelfCommentary = dbContext.ArticleCommentaries.Include(x => x.Article).Include(x => x.Author).Where(x => x.ID == idSelfComment).First();
            if (SelfCommentary == null)
                return RedirectToAction("Index", "Home");
            ArticleCommentaries newComment = new ArticleCommentaries
            {
                Content = txt,
                Author = user,
                DateSend = DateTime.Now,
                Article = SelfCommentary.Article,
                ParentCommentary = SelfCommentary
            };
            dbContext.ArticleCommentaries.Add(newComment);
            SelfCommentary.ChildsCommentary.Add(newComment);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("CurrentArticle/" + SelfCommentary.Article.ID.ToString(), "Home");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CurrentArticle(int id)
        {
                Article project = dbContext.Articles.Where(x => x.ID == id).FirstOrDefault();
                return View(project);
        }

        public IActionResult Article()
        {
            return View();
        }

        public IActionResult Developers()
        {
            return View(dbContext.Articles.ToList());
        }

        public IActionResult Gamers()
        {
            return View();
        }
    }
}
