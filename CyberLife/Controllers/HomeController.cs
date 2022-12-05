using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLife.Models;

namespace CyberLife.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public static Map map = new Map(1);
    private static bool start = true;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public void Restart()
    {
        map = new Map(1);
    }
    public IActionResult Main()
    {
        return View(map);
    }
    
    public IActionResult Start()
    {
        if (start)
        {
            start = false;
            map.Work();
        }
        start = true;
        return View(map);
    }
    
    public IActionResult Index()
    {
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