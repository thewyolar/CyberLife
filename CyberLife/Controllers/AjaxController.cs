using CyberLife.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberLife.Controllers;

public class AjaxController : Controller
{
    
    public static Dictionary<string, Map> Maps = new Dictionary<string, Map>();
    public static IList<string> Session = new List<string>();
    
    public IActionResult Restart()
    {
        Map map = Maps[HttpContext.Session.GetString(HttpContext.Session.Id)];
        Maps[HttpContext.Session.GetString(HttpContext.Session.Id)] = new Map(map.Width, map.Height, map.WidthBiome, map.SizeBiome, map.BotSpawnChance);
        return Redirect("Start");
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult SetMapParameters(int width = 78, int height = 40, int widthBiome = 30, int sizeBiome = 300, int botSpawnChance = 50)
    {
        Maps[HttpContext.Session.GetString(HttpContext.Session.Id)] = new Map(width, height, widthBiome, sizeBiome, botSpawnChance);
        return Redirect("Start");
    }
    
    public IActionResult Main()
    {
        if (!Maps.ContainsKey(HttpContext.Session.Id))
        {
            HttpContext.Session.SetString(HttpContext.Session.Id, HttpContext.Session.Id);
            Session.Add(HttpContext.Session.Id);
            Maps.Add(HttpContext.Session.Id, new Map());
        }
        return View(Maps[HttpContext.Session.GetString(HttpContext.Session.Id)]);
    }
    
    public IActionResult Start()
    {
        Maps[HttpContext.Session.GetString(HttpContext.Session.Id)].Work(HttpContext.Session.GetString(HttpContext.Session.Id));
        return View(Maps[HttpContext.Session.GetString(HttpContext.Session.Id)]);
    }

}