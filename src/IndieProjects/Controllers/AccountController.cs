using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using IndieProjects.Model;
using IndieProjects.ViewModel;

namespace IndieProjects.Controllers
{
    public class AccountController : Controller
    {
        IndieContext indieContext;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        IHostingEnvironment _appEnviroment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IndieContext context, IHostingEnvironment appEnvironment)
        {
            indieContext = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnviroment = appEnvironment;
        }

        public async Task<IActionResult> MyArticles()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Article> articles = indieContext.Articles.Where(x => x.Author == user).ToList();
            return View(articles);
        }
        [HttpGet]
        public async Task<JsonResult> MyProject()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Project> projects = indieContext.Projects.Where(x => x.ProjectManager == user).ToList();
            return Json(projects);
        }

        public async Task<IActionResult> Messages()
        {
            ViewBag.Author = ((User)(await _userManager.FindByNameAsync(User.Identity.Name))).Id;
            return View();
        }

        [HttpPost]
        public string AddAvatar([FromBody] AvatarWithParameters format)
        {
            //var obj = JsonConvert.DeserializeObject<byte>(format);
            return "Успешно";
        }

        public IActionResult AddArticle()
        {
            return View();
        }

        public PartialViewResult Comments(int id)
        {
            List<Commentary> comments = indieContext.Commentaries.Include(x => x.Author).Include(x => x.Article).Where(x => x.Article.ID == id).ToList();
            return PartialView(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(Article article)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            indieContext.Articles.Add(new Article()
            {
                Author = user,
                Content = article.Content,
                Title = article.Title,
                DateOfPublish = DateTime.Now,
                Commentaries = new List<Commentary>(),
                Likes = 0,
                Tags = new List<Tag>()
            });
            await indieContext.SaveChangesAsync();
            return RedirectToAction("MyArticles", "Account");
        }

        [HttpPost]
        /// <summary>
        /// Отправка сообщения
        /// </summary>
        public IActionResult SendMessage(Message mes)
        {
            return View();
        }

        public IActionResult Notification()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeInformation(User user)
        {
            User Currentuser = indieContext.Users.FirstOrDefault(x => x.Id == user.Id);
            Currentuser.City = user.City;
            Currentuser.Avatar = user.Avatar;
            Currentuser.AboutMe = user.Avatar;
            Currentuser.Country = user.Country;
            Currentuser.FIO = user.FIO;
            Currentuser.OwnSite = user.OwnSite;
            Currentuser.Skype = user.Skype;
            indieContext.SaveChangesAsync();
            return RedirectToAction("ProfileChanges");
        }

        public ActionResult AvatarEdit()
        {
           return PartialView();
        }

        public ActionResult AvatarEditAdd()
        {
            return PartialView();
        }

        public async Task<IActionResult> ProfileChanges()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }
        
        public async Task<IActionResult> MyProjects()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Project> projects = indieContext.Projects.Where(x => x.ProjectManager == user).ToList();
            return View(projects);
        }

        public IActionResult AboutMe()
        {
            return View();
        }

        public IActionResult MainAccountProfile()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> AccountProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Register");
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { NickName = model.NickName, Email = model.Email, UserName = model.Email };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
