namespace CyberLife.Models;

public class LayerModel : BaseModel
{
    public int IndexLayer { get; set; }
    public int[] FuncIndexNeuronsActivation { get; set; }
    
    public int[] FuncIndexDerivativeActivation  { get; set; }
    public int Size { get; set; }
    
    public double[] Neurons { get; set; }
    public double[] Biases { get; set; }
    public double[,] Weights { get; set; }

}