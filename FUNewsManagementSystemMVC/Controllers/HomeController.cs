using FUNewsManagementSystemMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using System.Diagnostics;

namespace FUNewsManagementSystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsArticleService _contextNewsArticle;
        public HomeController(ILogger<HomeController> logger, INewsArticleService contextNewsArticle)
        {
            _logger = logger;
            _contextNewsArticle = contextNewsArticle;
        }
        public IActionResult Admin()
        {
            return View();  
        }
        public IActionResult Index()
        {
            var activeNewsArticles = _contextNewsArticle.GetActiveNewsArticles();
            return View(activeNewsArticles.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Lecturer()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "SystemAccounts"); 
            }
            ViewBag.UserName = userName;
            return View();
        }

        public IActionResult Staff()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "SystemAccounts"); 
            }
            ViewBag.UserName = userName;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = _contextNewsArticle.GetNewsArticleById(id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }
    }
}
