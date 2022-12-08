using System.Diagnostics;
using CyberLife.Data;
using Microsoft.AspNetCore.Mvc;
using CyberLife.Models;

namespace CyberLife.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public IActionResult GetAllBot()
    {
        return View(_context.Perceptrons.ToList());
    }
    
    [HttpPost]
    public Task SaveBot(int x, int y, string name)
    {
        if (AjaxController.Maps[0].Bots[x, y] is null)
        {
            return Response.WriteAsJsonAsync("{ \"save\": false }");
        }
        _context.Perceptrons.Add(new PerceptronModel(AjaxController.Maps[0].Bots[x, y].Brain, name));
        _context.SaveChangesAsync();
        return Response.WriteAsJsonAsync("{ \"save\": true }");
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