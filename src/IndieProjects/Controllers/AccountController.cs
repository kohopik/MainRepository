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

        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        [HttpPost]
        public async Task<string> AddAvatar([FromBody] AvatarWithParameters parameter)
        {
            string base64 = parameter.Avatar.Remove(0, parameter.Avatar.IndexOf("base64") + 7);
            var bytes = Convert.FromBase64String(base64);
            MemoryStream mStream = new MemoryStream();
            await mStream.WriteAsync(bytes, 0, Convert.ToInt32(bytes.Length));
            Bitmap bm = new Bitmap(mStream, false);
            ImageFormat img = bm.RawFormat;
            if(bm.Width != parameter.width || bm.Height != parameter.height)
            {
                 bm = ResizeImage(bm, parameter.width, parameter.height);
            }
            Rectangle cropRect = new Rectangle(parameter.x1, parameter.y1, parameter.x2 - parameter.x1, parameter.y2 - parameter.y1);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(bm, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            User Currentuser = indieContext.Users.FirstOrDefault(x => x.Id == user.Id);
            string newPath = @"images/avatars/" + user.Id.ToString();
            if (ImageFormat.Jpeg.Equals(img))
            {
                target.Save(_appEnviroment.WebRootPath + "\\images\\avatars\\"+ Currentuser.Id.ToString() + ".jpeg" , System.Drawing.Imaging.ImageFormat.Jpeg);
                newPath += ".jpeg";
            }
            if (ImageFormat.Png.Equals(img))
            {
                target.Save(_appEnviroment.WebRootPath + "\\images\\avatars\\" + Currentuser.Id.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                newPath += ".png";
            }
            if(ImageFormat.Bmp.Equals(img))
            {
                target.Save(_appEnviroment.WebRootPath + "\\images\\avatars\\" + Currentuser.Id.ToString() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                newPath += ".bmp";
            }
            mStream.Dispose();
            bm.Dispose();
            target.Dispose();
            Currentuser.Avatar = newPath;
            await indieContext.SaveChangesAsync();
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
            return PartialView("_Activity");
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
            return PartialView("_About");
        }

        public PartialViewResult _AboutMe()
        {
            return PartialView();
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
        public async Task<IActionResult> Register(string Login,string Email,string Password, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                User user = new User { NickName = Login, Email = Email, UserName = Login };
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}
