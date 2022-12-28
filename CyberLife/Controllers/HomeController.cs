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
        List<User> user = _context.Users.Where(x => x.UserName.Equals(User.Identity.Name)).ToList();
        return View(_context.Perceptrons.Where(x => x.User.Id == user[0].Id).ToList());
    }
    
    [Authorize]
    public IActionResult GetFilteredBots(string name)
    {
        List<User> user = _context.Users.Where(x => x.UserName.Equals(User.Identity.Name)).ToList();
        IQueryable<PerceptronModel> bots = _context.Perceptrons.Where(bot => bot.User.Id == user[0].Id);
        return View(bots.Where(bot => bot.Name.Contains(name)).ToList());
    }
    
    [Authorize]
    public IActionResult DeleteBot(string perceptronId)
    {
        List<User> user = _context.Users.Where(x => x.UserName.Equals(User.Identity.Name)).ToList();
        List<LayerModel> layer = _context.Layers.Where(x => x.PerceptronModel.Id == Guid.Parse(perceptronId)).ToList();
        PerceptronModel perceptron = _context.Perceptrons.Find(Guid.Parse(perceptronId));
        if (user[0].Id.Equals(perceptron.User.Id))
        {
            _context.Layers.RemoveRange(layer);
            _context.Perceptrons.Remove(perceptron);
            _context.SaveChanges();
        }
        return Redirect("GetAllBot");
    }
    
    [Authorize]
    public IActionResult ChangeBot(string perceptronId, string name)
    {
        List<User> user = _context.Users.Where(x => x.UserName.Equals(User.Identity.Name)).ToList();
        PerceptronModel perceptron = _context.Perceptrons.Find(Guid.Parse(perceptronId));
        perceptron.Name = name;
        if (user[0].Id.Equals(perceptron.User.Id))
        {
            _context.Perceptrons.Update(perceptron);
            _context.SaveChanges();
        }
        return Redirect("GetAllBot");
    }
    
    [Authorize]
    [HttpPost]
    public Task SaveBot(int x, int y, string name)
    {
        if (AjaxController.Maps[HttpContext.Session.Id].Bots[x, y] is null)
        {
            return Response.WriteAsJsonAsync("{ \"save\": false }");
        }
        List<User> user = _context.Users.Where(x => x.UserName.Equals(User.Identity.Name)).ToList();
        _context.Perceptrons.Add(new PerceptronModel(AjaxController.Maps[HttpContext.Session.GetString(HttpContext.Session.Id)].Bots[x, y].Brain, name, user[0]));
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
            AjaxController.Maps[HttpContext.Session.GetString(HttpContext.Session.Id)].Bots[x, y] = new Bot(perceptron);
        }
        AjaxController.Maps[HttpContext.Session.GetString(HttpContext.Session.Id)].AddType(perceptron);
    }
    
    [Authorize]
    public IActionResult GetAllMaps()
    {
        return View(_context.Maps.ToList());
    }

    [Authorize]
    public IActionResult GetFilteredMaps(string name)
    {
        return View(_context.Maps.Where(map => map.Name.Contains(name)).ToList());
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteMap(string mapId)
    {
        MapModel map = _context.Maps.Find(Guid.Parse(mapId));
        _context.Maps.Remove(map);
        _context.SaveChanges();
        return Redirect("GetAllMaps");
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult ChangeMap(string mapId, string name)
    {
        MapModel map = _context.Maps.Find(Guid.Parse(mapId));
        map.Name = name;
        _context.Maps.Update(map);
        _context.SaveChanges();
        return Redirect("GetAllMaps");
    }
    
    [Authorize(Roles = "Admin")]
    public Task SaveMap(string name)
    {
        _context.Maps.Add(new MapModel(AjaxController.Maps[HttpContext.Session.GetString(HttpContext.Session.Id)].MapTypes, name));
        _context.SaveChanges();
        return Response.WriteAsJsonAsync("{ \"save\": true }");
    }

    [Authorize]
    public RedirectResult loadMap(string mapId)
    {
        IList<MapModel> mapModels = _context.Maps.Where(x => x.Id == Guid.Parse(mapId)).ToList();
        AjaxController.Maps[HttpContext.Session.GetString(HttpContext.Session.Id)].MapTypes = mapModels[0].MapTypes;
        AjaxController.Maps[HttpContext.Session.GetString(HttpContext.Session.Id)].ChangeColorMap(mapModels[0].MapTypes);
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