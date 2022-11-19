using System.Drawing;
using CyberLife.Neuronet;

namespace CyberLife.Models;

public class Bot: BaseModel
{
    public string color { get; set; }
    public string[] rgb { get; set; }
    public long energy { get; set; }
    public int[] eye { get; set; }
    public Perceptron brain { get; set; }

    public int isStep { get; set; } = 0;

    private Bot(long energy, Perceptron brain, string color, int isStep)
    {
        if (isStep == 1)
        {
            this.isStep = 0;
        }else if (isStep == 0)
        {
            this.isStep = 1;
        }
        this.energy = energy;
        this.brain = brain;
        this.brain.allEnergy += this.energy;
        this.color = color;
        this.rgb = this.color.Split(" ");
        this.brain.population++;
    }
    
    public Bot()
    {
        energy = 1000;
        this.color = "0, 255, 0";
        this.rgb = this.color.Split(" ");
        brain = new Perceptron(9, 5, 5, 5, 5, 10);
        this.brain.allEnergy += this.energy;
        this.brain.population++;
    }

    public void activateBrain(int[] eye, int x, int y, Bot[,] bots, MapType[,] map)
    {
        double[] input = new double[]{energy};
        int step = brain.FeedForward(eye, input);
        if (energy <= 0)
        {
            bots[x, y] = null;
            death();
            return;
        }
        if (step == -1)
        {
            updateEnergy(-20);
            return;
        }
        brain.activatedNeurons[step]++;
        if (step <= 7)
        {
            move(step, x, y, bots);
        }else if (step == 8)
        {
            generation(x, y, map);
        }else if (step == 9)
        {
            updateEnergy(energy / 2);
            reproduction(x, y, energy, bots, brain , color, isStep);
        }
        updateEnergy(-10);
        if (energy <= 0)
        {
            bots[x, y] = null;
            death();
        }
        
    }

// Написать через eye.
    public void move(int step, int x, int y,  Bot[,] bots)
    {
        try
        {
            if (step == 0 & bots[x - 1, y - 1] is null)
            {
                bots[x - 1, y - 1] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (step == 1 & bots[x, y - 1] is null)
            {
                bots[x, y - 1] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (step == 2 & bots[x + 1, y - 1] is null)
            {
                bots[x + 1, y - 1] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 3 & bots[x + 1, y] is null)
            {
                bots[x + 1, y] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 4 & bots[x + 1, y + 1] is null)
            {
                bots[x + 1, y + 1] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 5 & bots[x, y + 1] is null)
            {
                bots[x, y + 1] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 6 & bots[x - 1, y + 1] is null)
            {
                bots[x - 1, y + 1] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 7 & bots[x - 1, y] is null)
            {
                bots[x - 1, y] = bots[x, y];
                bots[x, y] = null;
                return;
            }
        }
        catch (Exception ignore) { }
        updateEnergy(-10);
    }

    public void generation(int x, int y, MapType[,] map)
    {
        updateEnergy((long) (14 + map[x, y]));
    }

    public void death()
    {
        if (brain.population <= 0)
        {
            brain.population = 0;
        }
        else
        {
            brain.population--;
        }
    }

    public void updateEnergy(long energyLoss)
    {
        this.energy += energyLoss;
        if (this.energy <= 0)
        {
            this.energy -= energyLoss;
            brain.allEnergy -= this.energy;
            this.energy += energyLoss;
        }
        else
        {
            brain.allEnergy += energyLoss;
        }
    }
    
    public void reproduction(int x, int y, long energy, Bot[,] bots, Perceptron brain, string color, int isStep)
    {
        Random random = new Random();
        int mutationProbability = random.Next(100);
        if (mutationProbability <= 5)
        {
            brain = brain.makePerceptron();
            int r = random.Next(240);
            int g = random.Next(240);
            int b = random.Next(240);
            color = r + ", " + g + ", " + b;
        }
        try
        {
            if (bots[x - 1, y - 1] is null)
            {
                bots[x - 1, y - 1] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (bots[x, y - 1] is null)
            {
                bots[x, y - 1] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (bots[x + 1, y - 1] is null)
            {
                bots[x + 1, y - 1] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x + 1, y] is null)
            {
                bots[x + 1, y] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x + 1, y + 1] is null)
            {
                bots[x + 1, y + 1] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x, y + 1] is null)
            {
                bots[x, y + 1] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x - 1, y + 1] is null)
            {
                bots[x - 1, y + 1] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x - 1, y] is null)
            {
                bots[x - 1, y] = new Bot(energy, brain, color, isStep);
                return;
            }
        }
        catch (Exception ignore) { }
    }

}