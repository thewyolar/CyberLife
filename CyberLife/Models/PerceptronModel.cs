using CyberLife.Neuronet;

namespace CyberLife.Models;

public class PerceptronModel : BaseModel
{
    public string Name { get; set; }
    
    public LayerModel[] Layers { get; set; }
    public string[] RGB { get; set; }
    
}