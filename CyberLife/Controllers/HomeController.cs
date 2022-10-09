using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLife.Models;
using CyberLife.DAO;

namespace CyberLife.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private DAOA dao = new DAOA();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Main()
    {
        return View();
    }
    public IActionResult Index()
    {
        using (dao)
        {
            // получаем объекты из бд и выводим на консоль
            var users = dao.qqqq.ToList();
            foreach (Qqqq u in users)
            {
                Console.WriteLine($"{u.www}");
            }
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}