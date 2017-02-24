using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IndieProjects.Model;
using Microsoft.AspNetCore.Identity;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using IndieProjects.ViewModel;
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
            Project myProject = new Project()
            {
                ProjectManager = user,
                Links = project.Links,
                Description = project.Description,
                StatusProject = project.StatusProject,
                Name = project.Name,
                Likes = 0,
                Team = new List<DeveloperProject>(),
                Vakancies = new List<Vakanci>()
            };
            context.Projects.Add(myProject);
            await context.SaveChangesAsync();
            user.Projects.Add(new DeveloperProject()
            {
                Project = myProject,
                User = user
            });
            Project proj = context.Projects.Where(x => x.Name == project.Name).First();
            AddAvatar(project.Avatar,proj);
            return RedirectToAction("MyProjects","Account");
        }

        [HttpPost]
        public async void AddAvatar(string Avatar,Project project)
        {
            string base64 = Avatar.Remove(0, Avatar.IndexOf("base64") + 7);
            var bytes = Convert.FromBase64String(base64);
            MemoryStream mStream = new MemoryStream();
            await mStream.WriteAsync(bytes, 0, Convert.ToInt32(bytes.Length));
            string newPath = @"images/projects/" + project.ProjectID.ToString();
            using (MagickImage image = new MagickImage(mStream))
            {
                switch (image.Format)
                {
                    case MagickFormat.Jpeg:
                        image.Write(_appEnviroment.WebRootPath + "\\images\\projects\\" + project.ProjectID.ToString() + ".jpeg");
                        newPath += ".jpeg";
                        break;
                    case MagickFormat.Jpg:
                        image.Write(_appEnviroment.WebRootPath + "\\images\\projects\\" + project.ProjectID.ToString() + ".jpg");
                        newPath += ".jpg";
                        break;
                    case MagickFormat.Png:
                        image.Write(_appEnviroment.WebRootPath + "\\images\\projects\\" + project.ProjectID.ToString() + ".png");
                        newPath += ".png";
                        break;
                    case MagickFormat.Bmp:
                        image.Write(_appEnviroment.WebRootPath + "\\images\\projects\\" + project.ProjectID.ToString() + ".bmp");
                        newPath += ".bmp";
                        break;
                    case MagickFormat.Gif:
                        image.Write(_appEnviroment.WebRootPath + "\\images\\projects\\" + project.ProjectID.ToString() + ".gif");
                        newPath += ".gif";
                        break;
                }
            }
            mStream.Dispose();
            project.Avatar = newPath;
            await context.SaveChangesAsync();
        }

        [HttpGet]
        public IActionResult CurrentProjectPage(int id)
        {
            Project project = context.Projects.Where(x => x.ProjectID == id).FirstOrDefault();
            return View(project);
        }

        public PartialViewResult Comments(int id)
        {
            List<ProjectCommentaries> comments = context.ProjectCommentaries.Include(x => x.Author).Include(x => x.Project).Where(x => x.Project.ProjectID == id).ToList();
            return PartialView(comments);
        }

        public async Task<IActionResult> AddCommentToProject(string txt, int id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Project currentProject = context.Projects.FirstOrDefault(x => x.ProjectID == id);
            context.ProjectCommentaries.Add(new ProjectCommentaries
            {
                Content = txt,
                Author = user,
                DateSend = DateTime.Now,
                Project = currentProject
            });
            await context.SaveChangesAsync();
            return RedirectToAction("CurrentProjectPage/" + id.ToString(),"Project");
        }

        public async Task<IActionResult> AddSelfCommentToProject(string txt, int idSelfComment)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ProjectCommentaries SelfCommentary = context.ProjectCommentaries.Include(x => x.Project).Include(x => x.Author).Where(x => x.ID == idSelfComment).First();
            if (SelfCommentary == null)
                return RedirectToAction("Index", "Home");
            ProjectCommentaries newComment = new ProjectCommentaries
            {
                Content = txt,
                Author = user,
                DateSend = DateTime.Now,
                Project = SelfCommentary.Project,
                ParentCommentary = SelfCommentary
            };
            context.ProjectCommentaries.Add(newComment);
            SelfCommentary.ChildsCommentary.Add(newComment);
            await context.SaveChangesAsync();
            return RedirectToAction("CurrentProjectPage/" + SelfCommentary.Project.ProjectID.ToString(), "Project");
        }
    }
}
