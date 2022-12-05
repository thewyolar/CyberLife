namespace CyberLife.Models;
public enum MapType
{
    DESERT = 2,
    WATER = 0,
    TROPIC = 7,
    TEMPERATE = 6,
    TAIGA = 3,
    TUNDRA = 1
    
}


public class MapModel : BaseModel
{
    
    public string Name { get; set; }
    public int[,] MapTypes { get; set; }
    
}