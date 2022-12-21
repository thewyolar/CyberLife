namespace CyberLife.Models;


public class MapModel : BaseModel
{
    public string Name { get; set; } = "map";
    public User User { get; set; }
    public int[,] MapTypes { get; set; }
    
    public MapModel() {}
    
    public MapModel(int[,] mapType, string name, User user)
    {
        this.MapTypes = mapType;
        this.Name = name;
        this.User = user;
    }
}