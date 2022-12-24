using CyberLife.Models;

namespace CyberLife.Neuronet;

public class Layer : LayerModel, IComparable<Layer>
{
    public double[] Neurons { get; set; }
    public Func<double, double>[]  NeuronsActivation { get; set; }
    public Func<double, double>[]  DerivativeActivation { get; set; }

    private static readonly Func<double, double>[] Activation = { (x) => (1 / (1 + Math.Exp(-x * 10))),
                                                                (x) => (1 / (1 + Math.Exp(-x))),
                                                                (x) => (Math.Pow(Math.Sin(10000 * x), 2))
                                                                
    };
    private static readonly Func<double, double>[] Derivative = { (y) => ((10 * Math.Exp(10 * y)) / (Math.Pow((Math.Exp(10 * y) + 1), 2))),
                                                                (y) => (Math.Exp(y) / (Math.Pow((Math.Exp(y) + 1), 2))),
                                                                (y) => (20000 * Math.Sin(10000 * y) * Math.Cos(10000 * y))
                                                                
    };

    public Layer(int size, int nextSize, int indexLayer)
    {
        this.IndexLayer = indexLayer;
        this.Size = size;
        this.Neurons = new double[size];
        this.Biases = new double[size];
        this.Weights = new double[size, nextSize];
        this.NeuronsActivation = new Func<double, double>[nextSize];
        this.DerivativeActivation = new Func<double, double>[nextSize];
        this.FuncIndex = new int[nextSize];
        for (int i = 0; i < NeuronsActivation.Length; i++)
        {
            int j = new Random().Next(100);
            if (j <= 45)
            {
                NeuronsActivation[i] = Activation[0];
                DerivativeActivation[i] = Derivative[0];
                FuncIndex[i] = 0;
            } else if (j <= 95)
            {
                NeuronsActivation[i] = Activation[1];
                DerivativeActivation[i] = Derivative[1];
                FuncIndex[i] = 1;
            }else
            {
                NeuronsActivation[i] = Activation[2];
                DerivativeActivation[i] = Derivative[2];
                FuncIndex[i] = 2;
            }
        }
    }
    private Layer(int indexLayer, int size, double[] neurons, Func<double, double>[] neuronsActivation, 
        Func<double, double>[] derivativeActivation,double[] biases, double[,] weights, int[] funcIndex)
    {
        this.IndexLayer = indexLayer;
        this.Size = size;
        this.Neurons = neurons;
        this.NeuronsActivation = neuronsActivation;
        this.DerivativeActivation = derivativeActivation;
        this.Biases = biases;
        this.Weights = weights;
        this.FuncIndex = funcIndex;
    }
    
    public Layer(LayerModel layerModel)
    {
        this.IndexLayer = layerModel.IndexLayer;
        this.Size = layerModel.Size;
        this.Neurons = new double[this.Size];
        this.FuncIndex = layerModel.FuncIndex;
        this.DerivativeActivation = new Func<double, double>[layerModel.FuncIndex.Length];
        this.NeuronsActivation = new Func<double, double>[layerModel.FuncIndex.Length];
        for (int i = 0; i < layerModel.FuncIndex.Length; i++)
        {
            this.DerivativeActivation[i] = Derivative[layerModel.FuncIndex[i]];
            this.NeuronsActivation[i] = Activation[layerModel.FuncIndex[i]];
        }
        this.Biases = layerModel.Biases;
        this.Weights = layerModel.Weights;
    }
    
    public Layer Clone()
    {
        int index1 = this.IndexLayer;
        int size = this.Size;
        double[] neurons = new double[this.Neurons.Length];
        this.Neurons.CopyTo(neurons, 0);
        Func<double, double>[] neuronsActivation = new Func<double, double>[this.NeuronsActivation.Length];
        Func<double, double>[] derivativeActivation = new Func<double, double>[this.DerivativeActivation.Length];
        int[] funcIndex = new int[this.FuncIndex.Length];
        for (int i = 0; i < this.NeuronsActivation.Length; i++)
        {
            neuronsActivation[i] = (Func<double, double>) this.NeuronsActivation[i].Clone();
            funcIndex[i] = this.FuncIndex[i];
            derivativeActivation[i] = (Func<double, double>) this.DerivativeActivation[i].Clone();
        }
        double[] biases = new double[this.Biases.Length];
        double[,] weights = new double[this.Weights.GetLength(0),this.Weights.GetLength(1)];
        this.Biases.CopyTo(biases, 0);
        for (int i = 0; i < this.Weights.GetLength(0); i++)
        {
            for (int j = 0; j < this.Weights.GetLength(1); j++)
            {
                weights[i, j] = this.Weights[i, j];
            }
        }
        return new Layer(index1, size, neurons, neuronsActivation, derivativeActivation, biases, 
            weights, funcIndex);
    }
    
    
    public void Mutation()
    {
        if (NeuronsActivation.Length == 0) return;
        Random random = new Random();
        for (int i = 0; i < 2; i++)
        {
            int indexFunc = random.Next(3);
            int indexNeuronsActivation = random.Next(NeuronsActivation.Length);
            NeuronsActivation[indexNeuronsActivation] = Activation[indexFunc];
            DerivativeActivation[indexNeuronsActivation] = Derivative[indexFunc];
            FuncIndex[indexNeuronsActivation] = indexFunc;
        }
    }
    
    public int CompareTo(Layer? other)
    {
        if (this.IndexLayer < other.IndexLayer) {return -1;}
        return this.IndexLayer > other.IndexLayer ? 1 : 0;
    }
}