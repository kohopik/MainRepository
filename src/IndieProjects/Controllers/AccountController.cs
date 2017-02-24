using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ImageMagick;
using IndieProjects.Model;
using IndieProjects.ViewModel;

namespace IndieProjects.Controllers
{
    public class AccountController : Controller
    {
        IndieContext context;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        IHostingEnvironment _appEnviroment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IndieContext context, IHostingEnvironment appEnvironment)
        {
            this.context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnviroment = appEnvironment;
        }

        public async Task<PartialViewResult> _MyArticles()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<Article> articles = context.Articles.Where(x => x.Author == user).ToList();
                return PartialView(articles);
        }

        public async Task<IActionResult> Messages()
        {
            ViewBag.Author = ((User)(await _userManager.FindByNameAsync(User.Identity.Name))).Id;
            return View();
        }

        [HttpPost]
        public async Task<string> AddAvatar([FromBody] AvatarWithParameters parameter)
        {
                string base64 = parameter.Avatar.Remove(0, parameter.Avatar.IndexOf("base64") + 7);
                var bytes = Convert.FromBase64String(base64);
                MemoryStream mStream = new MemoryStream();
                await mStream.WriteAsync(bytes, 0, Convert.ToInt32(bytes.Length));
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                User Currentuser = context.Users.FirstOrDefault(x => x.Id == user.Id);
                string newPath = @"images/avatars/" + user.Id.ToString();
                using (MagickImage image = new MagickImage(mStream))
                {
                    image.Resize(parameter.width, parameter.height);
                    image.Crop(parameter.x1, parameter.y1, parameter.x2 - parameter.x1, parameter.y2 - parameter.y1);
                    switch(image.Format)
                    {
                        case MagickFormat.Jpeg:
                            image.Write(_appEnviroment.WebRootPath + "\\images\\avatars\\" + Currentuser.Id.ToString() + ".jpeg");
                            newPath += ".jpeg";
                            break;
                        case MagickFormat.Jpg:
                            image.Write(_appEnviroment.WebRootPath + "\\images\\avatars\\" + Currentuser.Id.ToString() + ".jpg");
                            newPath += ".jpg";
                            break;
                        case MagickFormat.Png:
                            image.Write(_appEnviroment.WebRootPath + "\\images\\avatars\\" + Currentuser.Id.ToString() + ".png");
                            newPath += ".png";
                            break;
                        case MagickFormat.Bmp:
                            image.Write(_appEnviroment.WebRootPath + "\\images\\avatars\\" + Currentuser.Id.ToString() + ".bmp");
                            newPath += ".bmp";
                            break;
                        case MagickFormat.Gif:
                            image.Write(_appEnviroment.WebRootPath + "\\images\\avatars\\" + Currentuser.Id.ToString() + ".gif");
                            newPath += ".gif";
                            break;
                    }
                }
                mStream.Dispose();
                Currentuser.Avatar = newPath;
                await context.SaveChangesAsync();
                
                return "Успешно";
        }

        public IActionResult AddArticle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(Article article)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Article _article = new Article()
            {
                Author = user,
                Content = article.Content,
                Title = article.Title,
                DateOfPublish = DateTime.Now,
                Commentaries = new List<ArticleCommentaries>(),
                Likes = 0,
                Tags = new List<Tag>()
            };
            context.Articles.Add(_article);
            user.Articles.Add(_article);
            await context.SaveChangesAsync();
            return RedirectToAction("MainAccountProfile", "Account");
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
            return PartialView("_Activity");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeInformation(User user)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
                User Currentuser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (Currentuser != null)
                {
                    Currentuser.City = user.City;
                    Currentuser.Avatar = user.Avatar;
                    Currentuser.AboutMe = user.Avatar;
                    Currentuser.Country = user.Country;
                    Currentuser.FIO = user.FIO;
                    Currentuser.OwnSite = user.OwnSite;
                    Currentuser.Skype = user.Skype;
                    await _userManager.UpdateAsync(Currentuser);
                    return RedirectToAction("MainAccountProfile");
                }
                else
                    return RedirectToAction("Error");
        }

        public async Task<IActionResult> ProfileChanges()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
                return View(user);
        }

        public async Task<IActionResult> MainAccountProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
                user = context.Users.Include(x => x.Articles).Include(x => x.Projects).Include(x => x.Messages).Where(x => x.Id == user.Id).First();
                return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> AccountProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Login,string Email,string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                User user = new User { NickName = Login, Email = Email, UserName = Login, Avatar = @"\images\static\Noname.png" };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, Password);
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
            return View();
        }

        public async Task<PartialViewResult> _MyProjects()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Project> projects = context.Projects.Where(x => x.ProjectManager == user).ToList();
            return PartialView(projects);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email,string Password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Email, Password, false, false);
                if (result.Succeeded)
                {
                   return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}
