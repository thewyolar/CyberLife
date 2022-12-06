using CyberLife.Neuronet;

namespace CyberLife.Models;

public class PerceptronModel : BaseModel
{
    public string Name { get; set; } = "bot";
    public IList<LayerModel> Layers { get; set; }
    public string[] RGB { get; set; }
    
}