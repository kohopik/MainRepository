using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IndieProjects.Model;
using IndieProjects.Help;

namespace IndieProjects.Controllers
{
    public class HomeController : Controller
    {
        IndieContext context;
        UserActions actions;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public HomeController(IndieContext context,UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            actions = new UserActions();
        }

        [HttpPost]
        public async Task<string> LikeArticle(int id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            int? result = await actions.AddLike(context, user, id, LikeStatus.Article);
            return result != null ? result.ToString() : "Error";
        }

        public PartialViewResult Comments(int id)
        {
            List<ArticleCommentaries> comments = context.ArticleCommentaries.Include(x => x.Author).Include(x => x.Article).Where(x => x.Article.ID == id).ToList();
            return PartialView(comments);
        }

        public async Task<IActionResult> AddCommentToArticle(string txt, int id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Article currentArticle = context.Articles.FirstOrDefault(x => x.ID == id);
            context.ArticleCommentaries.Add(new ArticleCommentaries
            {
                Content = txt,
                Author = user,
                DateSend = DateTime.Now,
                Article = currentArticle
            });
            await context.SaveChangesAsync();
            return RedirectToAction("CurrentArticle/" + id.ToString(), "Home");
        }

        public async Task<IActionResult> CurrentUserProfile(string id)
        {
            User user = await context.Users.Where(x => x.Id == id).FirstAsync();
            return View(user);
        }

        public async Task<IActionResult> AddSelfCommentToArticle(string txt, int idSelfComment)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ArticleCommentaries SelfCommentary = context.ArticleCommentaries.Include(x => x.Article).Include(x => x.Author).Where(x => x.ID == idSelfComment).First();
            if (SelfCommentary == null)
                return RedirectToAction("Index", "Home");
            ArticleCommentaries newComment = new ArticleCommentaries
            {
                Content = "<a class=\"media-left\" href=\"/Home/CurrentUserProfile/" + SelfCommentary.Author.Id + "\"" + ">" + SelfCommentary.Author.NickName + "</a>," + txt,
                Author = user,
                DateSend = DateTime.Now,
                Article = SelfCommentary.Article,
                ParentCommentary = SelfCommentary
            };
            context.ArticleCommentaries.Add(newComment);
            SelfCommentary.ChildsCommentary.Add(newComment);
            await context.SaveChangesAsync();
            return RedirectToAction("CurrentArticle/" + SelfCommentary.Article.ID.ToString(), "Home");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CurrentArticle(int id)
        {
                Article project = context.Articles.Where(x => x.ID == id).FirstOrDefault();
                return View(project);
        }

        public IActionResult Article()
        {
            return View();
        }

        public IActionResult Developers()
        {
            return View(context.Articles.ToList());
        }

        public IActionResult Gamers()
        {
            return View();
        }
    }
}
