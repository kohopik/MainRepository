using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IndieProjects.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using IndieProjects.ViewModel;
using System.Drawing.Drawing2D;
using Microsoft.EntityFrameworkCore;


namespace IndieProjects.Controllers
{
    public class ProjectController : Controller
    {
        IndieContext context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        IHostingEnvironment _appEnviroment;
        public ProjectController(UserManager<User> userManager, SignInManager<User> signInManager, IndieContext indie, IHostingEnvironment appEnvironment)
        {
            context = indie;
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnviroment = appEnvironment;
        }

        public IActionResult AllProjects()
        {
            ViewBag.Projects = context.Projects.ToList();
            return View();
        }

        public IActionResult AllSummaries()
        {
            return View();
        }

        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(Project project)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            string Avatar = await AddAvatar(project.Avatar);
            Project myProject = new Project()
            {
                ProjectManager = user,
                Avatar = Avatar,
                Links = project.Links,
                Description = project.Description,
                StatusProject = project.StatusProject,
                Name = project.Name,
                Likes = 0,
                Team = new List<DeveloperProject>(),
                Vakancies = new List<Vakanci>()
            };
            await context.SaveChangesAsync();
            return RedirectToAction("MyProjects","Account");
        }

        [HttpPost]
        public async Task<string> AddAvatar(string Avatar)
        {
            string base64 = Avatar.Remove(0, Avatar.IndexOf("base64") + 7);
            var bytes = Convert.FromBase64String(base64);
            MemoryStream mStream = new MemoryStream();
            await mStream.WriteAsync(bytes, 0, Convert.ToInt32(bytes.Length));
            Bitmap bm = new Bitmap(mStream, false);

            var destImage = new Bitmap(bm.Width, bm.Height);

            destImage.SetResolution(bm.HorizontalResolution, bm.VerticalResolution);

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
                    graphics.DrawImage(bm, new Rectangle(0,0,bm.Width,bm.Height), 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            ImageFormat img = bm.RawFormat;
            string newPath = @"images/projects/" + Avatar;
            if (ImageFormat.Jpeg.Equals(img))
            {
                destImage.Save(_appEnviroment.WebRootPath + "\\images\\projects\\" + Avatar + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                newPath += ".jpeg";
            }
            if (ImageFormat.Png.Equals(img))
            {
                destImage.Save(_appEnviroment.WebRootPath + "\\images\\projects\\" + Avatar + ".png", System.Drawing.Imaging.ImageFormat.Png);
                newPath += ".png";
            }
            if (ImageFormat.Bmp.Equals(img))
            {
                destImage.Save(_appEnviroment.WebRootPath + "\\images\\projects\\" + Avatar + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                newPath += ".bmp";
            }
            mStream.Dispose();
            bm.Dispose();
            destImage.Dispose();
            await context.SaveChangesAsync();
            return newPath;
        }

        [HttpGet]
        public IActionResult CurrentProjectPage(int id)
        {
            Project project = context.Projects.Where(x => x.ProjectID == id).FirstOrDefault();
            return View(project);
        }

        public PartialViewResult Comments(int id)
        {
            List<Commentary> comments = context.Commentaries.Include(x => x.Author).Include(x => x.Project).Where(x => x.Project.ProjectID == id).ToList();
            return PartialView(comments);
        }

        public async Task<IActionResult> AddCommentToProject(string txt, int id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Project currentProject = context.Projects.FirstOrDefault(x => x.ProjectID == id);
            context.Commentaries.Add(new Commentary
            {
                Content = txt,
                Author = user,
                DateSend = DateTime.Now,
                Project = currentProject
            });
            await context.SaveChangesAsync();
            return RedirectToAction("CurrentProjectPage/" + id.ToString(),"Project");
        }
    }
}
