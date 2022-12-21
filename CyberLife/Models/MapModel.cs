namespace CyberLife.Models;


public class MapModel : BaseModel
{

    public string Name { get; set; } = "map";
    public int[,] MapTypes { get; set; }
    
}