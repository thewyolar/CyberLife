using System.Diagnostics;
using CyberLife.Data;
using Microsoft.AspNetCore.Mvc;
using CyberLife.Models;

namespace CyberLife.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static readonly ApplicationDbContext _context = new ApplicationDbContext();
    public static Map map = new Map();
    private static bool start = true;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpPost]
    public Task SaveBot(int x, int y, string name)
    {
        if (map.Bots[x, y] is null)
        {
            return Response.WriteAsJsonAsync("{ \"save\": false }");
        }
        _context.Perceptrons.Add(new PerceptronModel(map.Bots[x, y].Brain, name));
        _context.SaveChangesAsync();
        return Response.WriteAsJsonAsync("{ \"save\": true }");
    }
    public IActionResult Restart()
    {
        map = new Map();
        return Redirect("Start");
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