namespace CyberLife.Neuronet;

public class Perceptron
{
    public Layer[] Layers { get; set; }
    public int qqq = 0;
    public long population { get; set; } = 0;
    public double allEnergy { get; set; } = 0;
    

    public Perceptron(params int[] sizes)
    {
        this.Layers = new Layer[sizes.Length];
        for (int i = 0; i < sizes.Length; i++)
        {
            int nextSize = 0;
            if (i < sizes.Length - 1) 
                nextSize = sizes[i + 1];
            Layers[i] = new Layer(sizes[i], nextSize);
            for (int j = 0; j < sizes[i]; j++)
            {
                Layers[i].Biases[j] = new Random().NextDouble() * 2.0 - 1.0;
                for (int k = 0; k < nextSize; k++) {
                    Layers[i].Weights[j, k] = new Random().NextDouble() * 2.0 - 1.0;
                }
            }
        }
    }

    public int FeedForward(int[] eye , double[] inputs)
    {
        double[] inputss = new Double[inputs.Length + eye.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputss[i] = inputs[i];
        }
        for (int i = inputs.Length; i < inputs.Length + eye.Length; i++)
        {
            inputss[i] = eye[i - inputs.Length];
        }
        Array.Copy(inputss, 0, Layers[0].Neurons, 0, inputss.Length);
        for (int i = 1; i < Layers.Length; i++)
        {
            Layer layer = Layers[i - 1];
            Layer layerNext = Layers[i];
            for (int j = 0; j < layerNext.Size; j++)
            {
                layerNext.Neurons[j] = 0;
                for (int k = 0; k < layer.Size; k++)
                {
                    layerNext.Neurons[j] += layer.Neurons[k] * layer.Weights[k, j];
                }
                
                layerNext.Neurons[j] += layer.Biases[j];
                layerNext.Neurons[j] = layer.NeuronsActivation[j](layerNext.Neurons[j]);


            }
        }

        for (int i = 0; i < Layers[Layers.Length - 1].Neurons.Length; i++)
        {
            Console.WriteLine(Layers[Layers.Length - 1].Neurons[i]);
        }

        double ma = Layers[Layers.Length - 1].Neurons.Max();
        int index = Array.FindLastIndex(Layers[Layers.Length - 1].Neurons, delegate(double i) { return i == ma; });
        for (int i = 0; i < Layers[Layers.Length - 1].Neurons.Length; i++)
        {
            if (Layers[Layers.Length - 1].Neurons[i] == ma & index != i)
            {
                Console.WriteLine(-1);
                return -1;
            }
        }
        Console.WriteLine(index);
        Console.WriteLine("population=" + population);
        return Array.FindLastIndex(Layers[Layers.Length - 1].Neurons, delegate(double i) { return i == ma;});
    }

    public void BackPropagation(double energy, long population)
    {
        double[] errors = new double[Layers[Layers.Length - 1].Size];
        for (int i = 0; i < Layers[Layers.Length - 1].Size; i++) {
            if (population < 0)
            {
                errors[i] += population - 5;
            }
            else if (population > 0)
            {
                errors[i] += population + 5;
            }
            else if(energy > 0)
            {
                errors[i] += 1;
            }
            else if (energy < 0)
            {
                errors[i] -= 1;
            }
            else
            {
                errors[i] -= 1;
            }

        }

        for (int k = Layers.Length - 2; k >= 0; k--)
        {
            Layer layer = Layers[k];
            Layer layerNext = Layers[k + 1];
            double[] errorsNext = new double[layer.Size];
            double[] gradients = new double[layerNext.Size];
            for (int i = 0; i < layerNext.Size; i++)
            {
                gradients[i] = errors[i] * layer.DerivativeActivation[i](Layers[k + 1].Neurons[i]);  // вернуть производную????
                gradients[i] *= 0.1;
            }

            double[,] deltas = new double[layerNext.Size, layer.Size];
            for (int i = 0; i < layerNext.Size; i++)
            {
                for (int j = 0; j < layer.Size; j++)
                {
                    deltas[i, j] = gradients[i] * layer.Neurons[j]; // тут треш. перемножение на 1
                }
            }

            for (int i = 0; i < layer.Size; i++)
            {
                errorsNext[i] = 0;
                for (int j = 0; j < layerNext.Size; j++)
                {
                    errorsNext[i] += layer.Weights[i, j] * errors[j];
                }
            }

            errors = new double[layer.Size];
            Array.Copy(errorsNext, 0, errors, 0, layer.Size);
            double[,] weightsNew = new double[layer.Weights.GetLength(0), layer.Weights.GetLength(1)];
            for (int i = 0; i < layerNext.Size; i++)
            {
                for (int j = 0; j < layer.Size; j++)
                {
                    weightsNew[j, i] = layer.Weights[j, i] + deltas[i, j];
                }
            }

            if (qqq == -1)
            {
                // Console.WriteLine("////////////////////////////////////////////////////////////////////////////////////////////////////////////");
                // for (int i = 0; i < layerNext.Weights.GetLength(0); i++)
                // {
                //     Console.WriteLine(i);
                //     for (int j = 0; j < layerNext.Weights.GetLength(1); j++)
                //     {
                //         Console.WriteLine(layerNext.Weights[i, j]);
                //     }
                // }
                Console.WriteLine("////////////////////////////////////////////////////////////////////////////////////////////////////////////");
                for (int i = 0; i < layer.Weights.GetLength(0); i++)
                {
                    Console.WriteLine(i);
                    for (int j = 0; j < layer.Weights.GetLength(1); j++)
                    {
                        Console.WriteLine(layer.Weights[i, j]);
                    }
                }
            }

            layer.Weights = weightsNew;
            qqq++;
        }
    }
}