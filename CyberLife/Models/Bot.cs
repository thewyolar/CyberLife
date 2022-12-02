using System.Drawing;
using CyberLife.Controllers;
using CyberLife.Neuronet;

namespace CyberLife.Models;

public class Bot: BaseModel
{
    public string color { get; set; }
    public string[] rgb { get; set; }
    public long energy { get; set; }
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
        this.brain.rgb = this.rgb;
        this.brain.population++;
    }
    
    public Bot(string color)
    {
        energy = 100000;
        this.color = color;
        this.rgb = this.color.Split(" ");
        brain = new Perceptron(9, 10, 10, 10, 10, 12);
        this.brain.rgb = this.rgb;
        this.brain.allEnergy += this.energy;
        this.brain.population++;
    }

    public void activateBrain(int[] eye, int x, int y, Bot[,] bots, int[,] map)
    {
        double[] input = new double[]{energy};
        int step = brain.FeedForward(eye, input);
        if (energy <= 0)
        {
            death(x, y, bots);
            return;
        }

        Random random = new Random();
        if (random.Next(100) > 98)
        {
            Console.Write(step + ":");
            step = random.Next(12);
            Console.WriteLine(step);
        }
        
        if (step == -1)
        {
            step = random.Next(12);
        }
        updateEnergy(-1);
        brain.activatedNeurons[step]++;
        if (step <= 9)
        {
            int[] xy = move(step, x, y, eye, bots);
            x = xy[0];
            y = xy[1];
        }else if (step == 10)
        {
            generation(x, y, map);
        }else if (step == 11)
        {
            long loseEnergy = -(energy / 2);
            updateEnergy(loseEnergy);
            reproduction(x, y, -loseEnergy, bots, brain, color, isStep);
        }
        if (energy <= 0 & bots[x, y] is null)
        {
            death(x, y, bots);
        }
        
    }

    public bool attack(int xAttacking, int yAttacking, int xDefensive, int yDefensive,  Bot[,] bots)
    {
        long energyA = bots[xAttacking, yAttacking].energy;
        long energyD = bots[xDefensive, yDefensive].energy;
        bots[xDefensive, yDefensive].updateEnergy(-(long) Math.Round(energyA / 1.5));
        bots[xAttacking, yAttacking].updateEnergy(-(long) Math.Round(energyD / 2.5));
        if (bots[xDefensive, yDefensive].energy <= 0)
        {
            bots[xAttacking, yAttacking].updateEnergy((long) Math.Round(energyD / 1.5));
            bots[xDefensive, yDefensive].death(xDefensive, yDefensive, bots);
            return true;
        }
        return false;
    }
// Написать через eye.
    public int[] move(int step, int x, int y, int[] eye,  Bot[,] bots)
    {
        if (step >= 8)
        {
            // Console.WriteLine(step);
            int incrementX = -1;
            int incrementY = -1;
            bool isAttack = false;
            for (int i = 0; i < 8; i++)
            {
                if (eye[i] > 0 & step == 8)
                {
                    isAttack = attack(x, y, x + incrementX, y + incrementY, bots);
                    break;
                }else if (eye[i] == 2 & step == 9)
                {
                    isAttack = attack(x, y, x + incrementX, y + incrementY, bots);
                    break;
                }
                if (i < 4 & incrementX != 1)
                {
                    incrementX++;
                }else if(incrementY != 1)
                {
                    incrementY++;
                }
                else
                {
                    if (incrementX == -1)
                    {
                        incrementY--;
                    }
                    else
                    {
                        incrementX--;
                    }
                }
            }
            
            if (isAttack)
            {
                bots[x + incrementX, y + incrementY] = bots[x, y];
                bots[x, y] = null;
                return new []{x + incrementX, y + incrementY};
            }

            return new []{x, y};
        }
        try
        {
            if (step == 0 & bots[x - 1, y - 1] is null)
            {
                bots[x - 1, y - 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x - 1, y - 1};
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (step == 1 & bots[x, y - 1] is null)
            {
                bots[x, y - 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x, y - 1};
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (step == 2 & bots[x + 1, y - 1] is null)
            {
                bots[x + 1, y - 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x + 1, y - 1};
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 3 & bots[x + 1, y] is null)
            {
                bots[x + 1, y] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x + 1, y};
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 4 & bots[x + 1, y + 1] is null)
            {
                bots[x + 1, y + 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x + 1, y + 1};
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 5 & bots[x, y + 1] is null)
            {
                bots[x, y + 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x, y + 1};
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 6 & bots[x - 1, y + 1] is null)
            {
                bots[x - 1, y + 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x - 1, y + 1};
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (step == 7 & bots[x - 1, y] is null)
            {
                bots[x - 1, y] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x - 1, y};
            }
        }
        catch (Exception ignore) { }
        updateEnergy(-10);
        return new int[] {x, y};
    }

    public void generation(int x, int y, int[,] map)
    {
        updateEnergy((long) (14 + map[x, y]));
    }

    public void death(int x, int y, Bot[,] bots)
    {
        bots[x, y] = null;
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
        bool isMutation = false;
        if (mutationProbability <= 5)
        {
            brain = brain.makePerceptron();
            isMutation = true;
            string[] rgb = color.Split(", ");
            int[] rgbInt = new int[3];
            for (int i = 0; i < 3; i++)
            {
                rgbInt[i] = int.Parse(rgb[i]);
            }
            if (rgbInt[0] < 250 & rgbInt[2] == 0)
            {
                rgbInt[0] += random.Next(100);
                if (rgbInt[0] > 254) { rgbInt[0] = 254; }
            } else if (rgbInt[1] > 0 & rgbInt[2] == 0)
            {
                rgbInt[1] -= random.Next(100);
                if (rgbInt[1] < 0) { rgbInt[1] = 0; }
            }else if (rgbInt[2] < 250 & rgbInt[1] == 0)
            {
                rgbInt[2] += random.Next(100);
                if (rgbInt[2] > 254) { rgbInt[2] = 254; }
            }else if (rgbInt[0] > 0)
            {
                rgbInt[0] -= random.Next(100);
                if (rgbInt[0] < 0) { rgbInt[0] = 0; }
            }else if (rgbInt[1] < 250)
            {
                rgbInt[1] += random.Next(100);
                if (rgbInt[1] > 254) { rgbInt[1] = 254; }
            }else if (rgbInt[2] > 0)
            {
                rgbInt[2] -= random.Next(100);
                if (rgbInt[2] < 0) { rgbInt[2] = 0; }
            }
            color = rgbInt[0] + ", " + rgbInt[1] + ", " + rgbInt[2];
            Console.WriteLine(color);
        }
        try
        {
            if (bots[x - 1, y - 1] is null)
            {
                bots[x - 1, y - 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (bots[x, y - 1] is null)
            {
                bots[x, y - 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore){ }
        try
        {
            if (bots[x + 1, y - 1] is null)
            {
                bots[x + 1, y - 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x + 1, y] is null)
            {
                bots[x + 1, y] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x + 1, y + 1] is null)
            {
                bots[x + 1, y + 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x, y + 1] is null)
            {
                bots[x, y + 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x - 1, y + 1] is null)
            {
                bots[x - 1, y + 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore) { }

        try
        {
            if (bots[x - 1, y] is null)
            {
                bots[x - 1, y] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    HomeController.map.addType(brain);
                }
                return;
            }
        }
        catch (Exception ignore) { }
        bots[x, y].updateEnergy(energy - 50);
        return;
    }

}