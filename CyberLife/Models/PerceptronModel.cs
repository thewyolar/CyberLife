using CyberLife.Neuronet;

namespace CyberLife.Models;

public class PerceptronModel : BaseModel
{
    public string Name { get; set; } = "bot";
    public IList<LayerModel> Layers { get; set; }
    public string[] RGB { get; set; }
    public User User { get; set; }
    public PerceptronModel(){}
    
    public PerceptronModel(Perceptron perceptron, string name, User user)
    {
        this.Name = name;
        this.Layers = perceptron.layers;
        this.RGB = perceptron.RGB;
        this.User = user;
    }

}