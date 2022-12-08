using CyberLife.Models;
using Microsoft.AspNetCore.Mvc;

namespace CyberLife.Controllers;

public class AjaxController : Controller
{
    
    public static IList<Map> Maps = new List<Map> {new Map()};
    private static bool start = true;


    public IActionResult Restart()
    {
        Maps[0] = new Map();
        return Redirect("Start");
    }
    public IActionResult Main()
    {
        return View(Maps[0]);
    }
    
    public IActionResult Start()
    {
        if (start)
        {
            start = false;
            Maps[0].Work();
            start = true;
        }
        return View(Maps[0]);
    }

}