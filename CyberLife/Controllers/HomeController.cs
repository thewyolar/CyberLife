using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLife.Models;
using CyberLife.DAO;

namespace CyberLife.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private DAOA dao = new DAOA();
    private static Map map = new Map(1);

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
        map.work();
        return View(map);
    }
    
    public IActionResult Index()
    {
        return Redirect("/Home/Main");
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