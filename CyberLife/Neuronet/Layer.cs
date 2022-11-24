﻿namespace CyberLife.Neuronet;

public class Layer
{
    public int Size { get; set; }
    public double[] Neurons { get; set; }
    public Func<double, double>[]  NeuronsActivation { get; set; }
    public Func<double, double>[]  DerivativeActivation { get; set; }
    private static Func<double, double>[] Activation { get; set; } = { (x) => (1 / (1 + Math.Exp(-x * 10000))),
                                                                (x) => (1 / (1 + Math.Exp(-x))),
                                                                (x) => (Math.Pow(Math.Sin(10000 * x), 2))
                                                                
    };
    private static Func<double, double>[] Derivative { get; set; } = { (y) => (10000 * y / (1 + y * y)),
                                                                (y) => (y / (1 + y * y)),
                                                                (y) => (20000 * Math.Sin(10000 * y) * Math.Cos(10000 * y))
                                                                
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

    public Layer(int size, double[] neurons, Func<double, double>[] neuronsActivation,
        Func<double, double>[] derivativeActivation,double[] biases, double[,] weights)
    {
        this.Size = size;
        this.Neurons = neurons;
        this.NeuronsActivation = neuronsActivation;
        this.DerivativeActivation = derivativeActivation;
        this.Biases = biases;
        this.Weights = weights;
    }

    public void mutation()
    {
        Random random = new Random();
        int indexFunc = random.Next(3);
        if (NeuronsActivation.Length != 0)
        {
            int indexNeuronsActivation = random.Next(NeuronsActivation.Length);
            NeuronsActivation[indexNeuronsActivation] = Activation[indexFunc];
            DerivativeActivation[indexNeuronsActivation] = Derivative[indexFunc];
        }
    }

    public Layer clone()
    {
        int size = this.Size;
        double[] neurons = new double[this.Neurons.Length];
        this.Neurons.CopyTo(neurons, 0);
        Func<double, double>[] neuronsActivation = new Func<double, double>[this.NeuronsActivation.Length];
        Func<double, double>[] derivativeActivation = new Func<double, double>[this.DerivativeActivation.Length];
        for (int i = 0; i < this.NeuronsActivation.Length; i++)
        {
            neuronsActivation[i] = (Func<double, double>) this.NeuronsActivation[i].Clone();
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
        return new Layer(size, neurons, neuronsActivation, derivativeActivation, biases, weights);
    }
    
}