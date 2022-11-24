using CyberLife.Models;

namespace CyberLife.Neuronet;

public class Perceptron : PerceptronModel
{
    public Layer[] Layers { get; set; }
    public int[] activatedNeurons { get; set; }
    public long population { get; set; } = 0;
    public long allEnergy { get; set; } = 0;
    

    public Perceptron(params int[] sizes)
    {
        population = 0;
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
        activatedNeurons = new int[sizes.Last()];
    }
    
    private Perceptron(Layer[] layers)
    {
        this.Layers = layers;
        activatedNeurons = new int[Layers.Last().Size];
        population = 0;
        allEnergy = 0;
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
                
                layerNext.Neurons[j] += layerNext.Biases[j];
                layerNext.Neurons[j] = layer.NeuronsActivation[j](layerNext.Neurons[j]);


            }
        }

        // for (int i = 0; i < Layers[Layers.Length - 1].Neurons.Length; i++)
        // {
        //     Console.WriteLine(Layers[Layers.Length - 1].Neurons[i]);
        // }

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
        // Console.WriteLine(index);
        // Console.WriteLine("population=" + population);
        return Array.FindLastIndex(Layers[Layers.Length - 1].Neurons, delegate(double i) { return i == ma;});
    }
    
    
    
    // TODO dropOut - рандомное выключение нейронов. решить проблему переобучения
    public void BackPropagation(double energy, long population)
    {
        int neuron = Array.FindLastIndex(activatedNeurons, delegate(int i) { return i == activatedNeurons.Max(); });

        // foreach (int i in activatedNeurons)
        // {
        //     Console.Write(i + ";");
        // }
        
        
        double[] errors = new double[Layers[Layers.Length - 1].Size];
        for (int i = 0; i < Layers[Layers.Length - 1].Size; i++) {
            if (population < 0)
            {
                errors[neuron] -= 5;
            }
            else if (population > 0)
            {
                errors[neuron] += 5;
            }
            else if(energy > 0)
            {
                errors[neuron] += 1;
            }
            else if (energy < 0)
            {
                errors[neuron] -= 1;
            }
            else
            {
                errors[neuron] -= 1;
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
                gradients[i] = errors[i] * layer.DerivativeActivation[i](Layers[k + 1].Neurons[i]);
                gradients[i] *= 0.1;
            }

            double[,] deltas = new double[layerNext.Size, layer.Size];
            for (int i = 0; i < layerNext.Size; i++)
            {
                for (int j = 0; j < layer.Size; j++)
                {
                    deltas[i, j] = gradients[i] * layer.Neurons[j];
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
            layer.Weights = weightsNew;
            for (int i = 0; i < activatedNeurons.Length; i++)
            {
                activatedNeurons[i] = 0;
            }
        }
    }

    public Perceptron makePerceptron()
    {
        Layer[] layers = new Layer[this.Layers.Length];
        for (int i = 0; i < this.Layers.Length; i++)
        {
            layers[i] = this.Layers[i].clone();
        }
        Random random = new Random();
        int indexLayers = random.Next(layers.Length);
        for (int i = 0; i < 4; i++)
        {
            layers[indexLayers].mutation();
        }
        return new Perceptron(layers);
    }
}