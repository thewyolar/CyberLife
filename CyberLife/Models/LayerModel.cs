namespace CyberLife.Models;

public class LayerModel
{
    
    public int[] funcIndexNeuronsActivation { get; set; }
    
    public int[] funcIndexDerivativeActivation  { get; set; }
    public int Size { get; set; }
    
    public double[] Neurons { get; set; }
    public double[] Biases { get; set; }
    public double[,] Weights { get; set; }
    
}