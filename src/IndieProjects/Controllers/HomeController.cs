using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IndieProjects.Model;

namespace IndieProjects.Controllers
{
    public class HomeController : Controller
    {
        IndieContext dbContext;

        public HomeController(IndieContext context)
        {
            dbContext = context;
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
            return View();
        }

        public IActionResult Gamers()
        {
            return View();
        }
    }
}
