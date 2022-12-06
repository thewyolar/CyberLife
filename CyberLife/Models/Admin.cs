namespace CyberLife.Models;

public class Admin : User
{
    
    public IList<MapModel> Maps { get; set; }
    
}