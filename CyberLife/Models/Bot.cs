using System.Drawing;
using CyberLife.Neuronet;

namespace CyberLife.Models;

public class Bot
{
    public Color color { get; set; }
    public double energy { get; set; }
    public int[] eye { get; set; }
    public Perceptron brain { get; set; }

    private Bot(double energy, Perceptron brain)
    {
        this.energy = energy;
        this.brain = brain;
        this.brain.allEnergy += this.energy;
        this.brain.population++;
    }
    
    public Bot()
    {
        energy = 100;
        brain = new Perceptron(9, 5, 5, 5, 5, 3);
        this.brain.allEnergy += this.energy;
        this.brain.population++;
    }

    public void activateBrain(int[] eye, int x, int y, Bot[,] bots, MapType[,] map)
    {
        double[] input = new double[]{energy};
        double step = brain.FeedForward(eye, input);
        
        updateEnergy(-10);
        if (step == 0)
        {
            move();
        }else if (step == 1)
        {
            generation(x, y, map);
        }else if (step == 2)
        {
            energy /= 2;
            reproduction(x, y, energy, bots, brain);
        }

        if (energy <= 0)
        {
            bots[x, y] = null;
            death();
        }
        
    }


    public void move()
    {
        updateEnergy(-10);
    }

    public void generation(int x, int y, MapType[,] map)
    {
        updateEnergy((int) (14 + map[x, y]));
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

    public void updateEnergy(int energyLoss)
    {
        this.energy += energyLoss;
        if (this.energy <= 0)
        {
            brain.allEnergy += (this.energy - energyLoss) - this.energy;
        }
        else
        {
            brain.allEnergy += energyLoss;
        }
    }
    
    public void reproduction(int x, int y, double energy, Bot[,] bots, Perceptron brain)
    {
        try
        {
            if (bots[x - 1, y - 1] is null) bots[x - 1, y - 1] = new Bot(energy, brain);
        }
        catch (Exception ignore){ }
        try
        {
            if (bots[x, y - 1] is null) bots[x, y - 1] = new Bot(energy, brain);
        }
        catch (Exception ignore){ }
        try
        {
            if (bots[x + 1, y + 1] is null) bots[x + 1, y + 1] = new Bot(energy, brain);
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x + 1, y] is null) bots[x + 1, y] = new Bot(energy, brain);
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x + 1, y - 1] is null) bots[x + 1, y - 1] = new Bot(energy, brain);
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x, y - 1] is null) bots[x, y - 1] = new Bot(energy, brain);
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x - 1, y + 1] is null) bots[x - 1, y + 1] = new Bot(energy, brain);
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x - 1, y] is null) bots[x - 1, y] = new Bot(energy, brain);
        }
        catch (Exception ignore) { }
    }

}