using System.Diagnostics;
using CyberLife.Data;
using Microsoft.AspNetCore.Mvc;
using CyberLife.Models;
using CyberLife.Neuronet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
    
    [Authorize]
    public IActionResult GetAllBot()
    {
        List<User> user = _context.Users.Where(x => x.UserName == User.Identity.Name).ToList();
        return View(_context.Perceptrons.Where(x => x.User.Id == user[0].Id).ToList());
    }

    [Authorize]
    [HttpPost]
    public Task SaveBot(int x, int y, string name)
    {
        if (AjaxController.Maps[0].Bots[x, y] is null)
        {
            return Response.WriteAsJsonAsync("{ \"save\": false }");
        }
        List<User> user = _context.Users.Where(x => x.UserName == User.Identity.Name).ToList();
        _context.Perceptrons.Add(new PerceptronModel(AjaxController.Maps[0].Bots[x, y].Brain, name, user[0]));
        _context.SaveChanges();
        return Response.WriteAsJsonAsync("{ \"save\": true }");
    }
    
    [Authorize]
    [HttpPost]
    public void LoadingBot(IList<string> bots)
    {
        IList<PerceptronModel> perceptronModels = _context.Perceptrons.Where
            ( x => x.Id==Guid.Parse(bots[0])).Include(x=>x.Layers).ToList();
        Layer[] layers = new Layer[perceptronModels[0].Layers.Count];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(perceptronModels[0].Layers[i]);
        }
        Array.Sort(layers);
        Perceptron perceptron = new Perceptron(layers);
        perceptron.RGB = perceptronModels[0].RGB;
        for (int i = 1; i < bots.Count; i++)
        {
            string[] xyStrings = bots[i].Split(",");
            int x = int.Parse(xyStrings[0]);
            int y = int.Parse(xyStrings[1]);
            AjaxController.Maps[0].Bots[x, y] = new Bot(perceptron);
        }
        AjaxController.Maps[0].AddType(perceptron);
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllMaps()
    {
        return View(_context.Maps.ToList());
    }
    
    [Authorize(Roles = "Admin")]
    public Task SaveMap(string name)
    {
        _context.Maps.Add(new MapModel(AjaxController.Maps[0].MapTypes, name));
        _context.SaveChanges();
        return Response.WriteAsJsonAsync("{ \"save\": true }");
    }

    [Authorize]
    public RedirectResult loadMap(string mapId)
    {
        IList<MapModel> mapModels = _context.Maps.Where(x => x.Id==Guid.Parse(mapId)).ToList();
        AjaxController.Maps[0].MapTypes = mapModels[0].MapTypes;
        AjaxController.Maps[0].ChangeColorMap();
        return Redirect("/Ajax/Start");
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Trends()
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