using CyberLife.Neuronet;

namespace CyberLife.Models;

public class PerceptronModel : BaseModel
{
    public LayerModel[] Layers { get; set; }
    public string[] rgb { get; set; }
    
}