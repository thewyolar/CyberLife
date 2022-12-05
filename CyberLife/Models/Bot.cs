using CyberLife.Controllers;
using CyberLife.Neuronet;

namespace CyberLife.Models;

public class Bot: BaseModel
{
    public string Color { get; set; }
    public string[] RGB { get; set; }
    public long Energy { get; set; }
    public Perceptron Brain { get; set; }
    public int IsStep { get; set; } = 0;

    private Bot(long energy, Perceptron brain, string color, int isStep)
    {
        if (isStep == 1)
        {
            this.IsStep = 0;
        }else if (isStep == 0)
        {
            this.IsStep = 1;
        }
        this.Energy = energy;
        this.Brain = brain;
        this.Brain.AllEnergy += this.Energy;
        this.Color = color;
        this.RGB = this.Color.Split(" ");
        this.Brain.RGB = this.RGB;
        this.Brain.Population++;
    }
    
    public Bot(string color)
    {
        Energy = 100;
        this.Color = color;
        this.RGB = this.Color.Split(" ");
        Brain = new Perceptron(9, 10, 10, 10, 10, 12);
        this.Brain.RGB = this.RGB;
        this.Brain.AllEnergy += this.Energy;
        this.Brain.Population++;
    }

    public void ActivateBrain(int[] eye, int x, int y, Bot[,] bots, int[,] map)
    {
        double[] input = new double[]{Energy};
        int step = Brain.FeedForward(eye, input);
        if (Energy <= 0)
        {
            Death(x, y, bots);
            return;
        }

        Random random = new Random();
        if (random.Next(100) > 98)
        {
            step = random.Next(12);
        }
        if (step == -1)
        {
            step = random.Next(12);
        }
        UpdateEnergy(-1);
        Brain.ActivatedNeurons[step]++;
        if (step <= 9)
        {
            int[] xy = Move(step, x, y, eye, bots);
            x = xy[0];
            y = xy[1];
        }else if (step == 10)
        {
            Generation(x, y, map);
        }else if (step == 11)
        {
            long loseEnergy = -(Energy / 2);
            UpdateEnergy(loseEnergy);
            Reproduction(x, y, -loseEnergy, bots, Brain, Color, IsStep);
        }
        if (Energy <= 0 & bots[x, y] is null)
        {
            Death(x, y, bots);
        }
        
    }

    public bool Attack(int xAttacking, int yAttacking, int xDefensive, int yDefensive,  Bot[,] bots)
    {
        long energyA = bots[xAttacking, yAttacking].Energy;
        long energyD = bots[xDefensive, yDefensive].Energy;
        bots[xDefensive, yDefensive].UpdateEnergy(-(long) Math.Round(energyA / 1.5));
        bots[xAttacking, yAttacking].UpdateEnergy(-(long) Math.Round(energyD / 2.5));
        if (bots[xDefensive, yDefensive].Energy <= 0)
        {
            bots[xAttacking, yAttacking].UpdateEnergy((long) Math.Round(energyD / 1.5));
            bots[xDefensive, yDefensive].Death(xDefensive, yDefensive, bots);
            return true;
        }
        return false;
    }

    public int[] Move(int step, int x, int y, int[] eye,  Bot[,] bots)
    {
        if (step >= 8)
        {
            int incrementX = -1;
            int incrementY = -1;
            bool isAttack = false;
            for (int i = 0; i < 8; i++)
            {
                if (eye[i] > 0 & step == 8)
                {
                    isAttack = Attack(x, y, x + incrementX, y + incrementY, bots);
                    break;
                }else if (eye[i] == 2 & step == 9)
                {
                    isAttack = Attack(x, y, x + incrementX, y + incrementY, bots);
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
        UpdateEnergy(-10);
        return new int[] {x, y};
    }

    public void Generation(int x, int y, int[,] map)
    {
        UpdateEnergy((long) (14 + map[x, y]));
    }

    public void Death(int x, int y, Bot[,] bots)
    {
        bots[x, y] = null;
        if (Brain.Population <= 0)
        {
            Brain.Population = 0;
        }
        else
        {
            Brain.Population--;
        }
    }

    public void UpdateEnergy(long energyLoss)
    {
        this.Energy += energyLoss;
        if (this.Energy <= 0)
        {
            this.Energy -= energyLoss;
            Brain.AllEnergy -= this.Energy;
            this.Energy += energyLoss;
        }
        else
        {
            Brain.AllEnergy += energyLoss;
        }
    }
    
    public void Reproduction(int x, int y, long energy, Bot[,] bots, Perceptron brain, string color, int isStep)
    {
        Random random = new Random();
        int mutationProbability = random.Next(100);
        bool isMutation = false;
        if (mutationProbability <= 1)
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
                if (rgbInt[0] > 254) { rgbInt[0] = 250; }
            } else if (rgbInt[1] > 0 & rgbInt[2] == 0)
            {
                rgbInt[1] -= random.Next(100);
                if (rgbInt[1] < 0) { rgbInt[1] = 0; }
            }else if (rgbInt[2] < 250 & rgbInt[1] == 0)
            {
                rgbInt[2] += random.Next(100);
                if (rgbInt[2] > 254) { rgbInt[2] = 250; }
            }else if (rgbInt[0] > 0)
            {
                rgbInt[0] -= random.Next(100);
                if (rgbInt[0] < 0) { rgbInt[0] = 0; }
            }else if (rgbInt[1] < 250)
            {
                rgbInt[1] += random.Next(100);
                if (rgbInt[1] > 254) { rgbInt[1] = 250; }
            }else if (rgbInt[2] > 0)
            {
                rgbInt[2] -= random.Next(100);
                if (rgbInt[2] < 0) { rgbInt[2] = 0; }
            }else
            {
                rgbInt[0] = random.Next(250);
                rgbInt[1] = random.Next(250);
                rgbInt[2] = random.Next(250);
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
                    HomeController.map.AddType(brain);
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
                    HomeController.map.AddType(brain);
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
                    HomeController.map.AddType(brain);
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
                    HomeController.map.AddType(brain);
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
                    HomeController.map.AddType(brain);
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
                    HomeController.map.AddType(brain);
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
                    HomeController.map.AddType(brain);
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
                    HomeController.map.AddType(brain);
                }
                return;
            }
        }
        catch (Exception ignore) { }
        bots[x, y].UpdateEnergy(energy - 50);
        return;
    }

}