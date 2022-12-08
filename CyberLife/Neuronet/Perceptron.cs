using CyberLife.Models;

namespace CyberLife.Neuronet;

public class Perceptron : PerceptronModel
{
    public Layer[] layers { get; set; }
    public int[] ActivatedNeurons { get; set; }
    public long Population { get; set; } = 0;
    public long AllEnergy { get; set; } = 0;
    

    public Perceptron(params int[] sizes)
    {
        Population = 0;
        this.layers = new Layer[sizes.Length];
        for (int i = 0; i < sizes.Length; i++)
        {
            int nextSize = 0;
            if (i < sizes.Length - 1) 
                nextSize = sizes[i + 1];
            layers[i] = new Layer(sizes[i], nextSize, i);
            for (int j = 0; j < sizes[i]; j++)
            {
                layers[i].Biases[j] = new Random().NextDouble() * 2.0 - 1.0;
                for (int k = 0; k < nextSize; k++) {
                    layers[i].Weights[j, k] = new Random().NextDouble() * 2.0 - 1.0;
                }
            }
        }
        ActivatedNeurons = new int[sizes.Last()];
    }
    
    public Perceptron(Layer[] layers)
    {
        this.layers = layers;
        ActivatedNeurons = new int[this.layers.Last().Size];
        Population = 0;
        AllEnergy = 0;
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
        Array.Copy(inputss, 0, layers[0].Neurons, 0, inputss.Length);
        for (int i = 1; i < layers.Length; i++)
        {
            Layer layer = layers[i - 1];
            Layer layerNext = layers[i];
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
        double ma = layers[layers.Length - 1].Neurons.Max();
        int index = Array.FindLastIndex(layers[layers.Length - 1].Neurons, delegate(double i) { return i == ma; });
        for (int i = 0; i < layers[layers.Length - 1].Neurons.Length; i++)
        {
            if (layers[layers.Length - 1].Neurons[i] == ma & index != i)
            {
                return -1;
            }
        }
        return Array.FindLastIndex(layers[layers.Length - 1].Neurons, delegate(double i) { return i == ma;});
    }
    
    
    
    // TODO dropOut - рандомное выключение нейронов. решить проблему переобучения
    public void BackPropagation(double energy, long population)
    {
        int neuron = Array.FindLastIndex(ActivatedNeurons, delegate(int i) { return i == ActivatedNeurons.Max();});
        double[] errors = new double[layers[layers.Length - 1].Size];
        for (int i = 0; i < layers[layers.Length - 1].Size; i++) {
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

        for (int k = layers.Length - 2; k >= 0; k--)
        {
            Layer layer = layers[k];
            Layer layerNext = layers[k + 1];
            double[] errorsNext = new double[layer.Size];
            double[] gradients = new double[layerNext.Size];
            for (int i = 0; i < layerNext.Size; i++)
            {
                gradients[i] = errors[i] * layer.DerivativeActivation[i](layers[k + 1].Neurons[i]);
                gradients[i] *= 0.01;
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
            for (int i = 0; i < ActivatedNeurons.Length; i++)
            {
                ActivatedNeurons[i] = 0;
            }
        }
    }

    public Perceptron makePerceptron()
    {
        Layer[] layers = new Layer[this.layers.Length];
        for (int i = 0; i < this.layers.Length; i++)
        {
            layers[i] = this.layers[i].clone();
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