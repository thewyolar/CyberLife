using CyberLife.Models;
using Microsoft.AspNetCore.Mvc;

namespace CyberLife.Controllers;

public class AjaxController : Controller
{
    
    public static Dictionary<string, Map> Maps = new Dictionary<string, Map>();
    public static IList<string> Session = new List<string>();
    
    public IActionResult Restart()
    {
        Maps[HttpContext.Session.GetString(HttpContext.Session.Id)] = new Map();
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