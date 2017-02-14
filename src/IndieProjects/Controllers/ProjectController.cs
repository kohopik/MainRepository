using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IndieProjects.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace IndieProjects.Controllers
{
    public class ProjectController : Controller
    {
        IndieContext context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ProjectController(UserManager<User> userManager, SignInManager<User> signInManager, IndieContext indie)
        {
            context = indie;
            _userManager = userManager;
            _signInManager = signInManager;
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
            context.Projects.Add(new Project()
            {
                ProjectManager = user,
                Avatar = project.Avatar,
                Links = project.Links,
                Description = project.Description,
                StatusProject = project.StatusProject,
                Name = project.Name,
                Likes = 0,
                Team = new List<DeveloperProject>(),
                Vakancies = new List<Vakanci>()});
            await context.SaveChangesAsync();
            return RedirectToAction("MyProjects","Account");
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
