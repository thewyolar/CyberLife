namespace CyberLife.Models;


public class MapModel : BaseModel
{
    public string Name { get; set; } = "map";
    public int[,] MapTypes { get; set; }
    public MapModel() {}
    
    public MapModel(int[,] mapType, string name)
    {
        this.MapTypes = mapType;
        this.Name = name;
    }
}