namespace CyberLife.Neuronet;

public class Layer
{
    public int Size { get; set; }
    public double[] Neurons { get; set; }
    public double[] Biases { get; set; }
    public double[,] Weights { get; set; }

    public Layer(int size, int nextSize)
    {
        this.Size = size;
        this.Neurons = new double[size];
        this.Biases = new double[size];
        this.Weights = new double[size, nextSize];
    }
}