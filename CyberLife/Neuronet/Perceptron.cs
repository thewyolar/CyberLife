namespace CyberLife.Neuronet;

public class Perceptron
{
    public double LearningRate { get; set; }
    public Layer[] Layers { get; set; }
    public Func<double, double> Activation { get; set; }
    public Func<double, double> Derivative { get; set; }

    public Perceptron(double learningRate, Func<double, double> activation, Func<double, double> derivative, params int[] sizes)
    {
        this.LearningRate = learningRate;
        this.Activation = activation;
        this.Derivative = derivative;
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

    public double[] FeedForward(double[] inputs)
    {
        Array.Copy(inputs, 0, Layers[0].Neurons, 0, inputs.Length);
        for (int i = 1; i < Layers.Length; i++)
        {
            Layer l = Layers[i - 1];
            Layer l1 = Layers[i];
            for (int j = 0; j < l1.Size; j++)
            {
                l1.Neurons[j] = 0;
                for (int k = 0; k < l.Size; k++)
                {
                    l1.Neurons[j] += l.Neurons[k] * l.Weights[k, j];
                }

                l1.Neurons[j] += l.Biases[j];
                l1.Neurons[j] = Activation(l1.Neurons[j]);
            }
        }

        return Layers[Layers.Length - 1].Neurons;
    }

    public void BackPropagation(double[] targets)
    {
        double[] errors = new double[Layers[Layers.Length - 1].Size];
        for (int i = 0; i < Layers[Layers.Length - 1].Size; i++) {
            errors[i] = targets[i] - Layers[Layers.Length - 1].Neurons[i];
        }
        for (int k = Layers.Length - 2; k >= 0; k--) {
            Layer l = Layers[k];
            Layer l1 = Layers[k + 1];
            double[] errorsNext = new double[l.Size];
            double[] gradients = new double[l1.Size];
            for (int i = 0; i < l1.Size; i++) {
                gradients[i] = errors[i] * Derivative(Layers[k + 1].Neurons[i]);
                gradients[i] *= LearningRate;
            }
            double[,] deltas = new double[l1.Size, l.Size];
            for (int i = 0; i < l1.Size; i++) {
                for (int j = 0; j < l.Size; j++) {
                    deltas[i, j] = gradients[i] * l.Neurons[j];
                }
            }
            for (int i = 0; i < l.Size; i++) {
                errorsNext[i] = 0;
                for (int j = 0; j < l1.Size; j++) {
                    errorsNext[i] += l.Weights[i, j] * errors[j];
                }
            }
            errors = new double[l.Size];
            Array.Copy(errorsNext, 0, errors, 0, l.Size);
            double[,] weightsNew = new double[l.Weights.Length, l.Weights.Length];
            for (int i = 0; i < l1.Size; i++) {
                for (int j = 0; j < l.Size; j++) {
                    weightsNew[j, i] = l.Weights[j, i] + deltas[i, j];
                }
            }
            l.Weights = weightsNew;
            for (int i = 0; i < l1.Size; i++) {
                l1.Biases[i] += gradients[i];
            }
        }
    }
}