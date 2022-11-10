namespace CyberLife.Neuronet;

public class Layer
{
    public int Size { get; set; }
    public double[] Neurons { get; set; }
    public Func<double, double>[]  NeuronsActivation { get; set; }
    public Func<double, double>[]  DerivativeActivation { get; set; }
    private Func<double, double>[] Activation { get; set; } = { (x) => (1 / (1 + Math.Exp(-x * 10000))),
                                                                (x) => (1 / (1 + Math.Exp(-x))),
                                                                (x) => (new Random().NextDouble())
                                                                
    };
    private Func<double, double>[] Derivative { get; set; } = { (y) => (10000 * y / (1 + y * y)),
                                                                (y) => (y / (1 + y * y)),
                                                                (y) => (y)
                                                                
    };
    public double[] Biases { get; set; }
    public double[,] Weights { get; set; }

    public Layer(int size, int nextSize)
    {
        this.Size = size;
        this.Neurons = new double[size];
        this.Biases = new double[size];
        this.Weights = new double[size, nextSize];
        this.NeuronsActivation = new Func<double, double>[nextSize];
        this.DerivativeActivation = new Func<double, double>[nextSize];
        for (int i = 0; i < NeuronsActivation.Length; i++)
        {
            int j = new Random().Next(100);
            if (j <= 45)
            {
                NeuronsActivation[i] = Activation[0];
                DerivativeActivation[i] = Derivative[0];
            }
            if (45 < j & j <= 95)
            {
                NeuronsActivation[i] = Activation[1];
                DerivativeActivation[i] = Derivative[1];
            }
            else
            {
                NeuronsActivation[i] = Activation[2];
                DerivativeActivation[i] = Derivative[2];
            }
        }
    }
}