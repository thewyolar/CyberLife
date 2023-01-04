using CyberLife.Controllers;
using CyberLife.Neuronet;

namespace CyberLife.Models;

public class Bot
{
    public string Color { get; set; }
    public string[] RGB { get; set; }
    private long Energy { get; set; }
    public Perceptron Brain { get; set; }
    public int IsStep { get; set; } = 0;
    private int isGenerator = 0;

    private Bot(long energy, Perceptron brain, string color, int isStep)
    {
        if (isStep == 1)
        {
            this.IsStep = 0;
        }else
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
        Brain = new Perceptron(9, 9, 9, 9, 9, 12);
        this.Brain.RGB = this.RGB;
        this.Brain.AllEnergy += this.Energy;
        this.Brain.Population++;
    }
    
    public Bot(Perceptron brain)
    {
        this.Energy = 100;
        this.Brain = brain;
        this.Brain.AllEnergy += this.Energy;
        this.RGB = brain.RGB;
        this.Color = RGB[0] + " " + RGB[1] + " " + RGB[2];
        this.Brain.Population++;
    }

    public void ActivateBrain(int[] eye, int x, int y, Bot[,] bots, int[,] map, string session)
    {
        double[] input = new double[]{Energy};
        int step = Brain.FeedForward(eye, input);
        if (Energy <= 0)
        {
            Death(x, y, bots);
            return;
        }
        Random random = new Random();
        if (random.Next(100) > 95)
        {
            step = random.Next(12);
            if (step == 8)
            {
                step++;
            }
        }
        if (step == -1)
        {
            UpdateEnergy(-1);
            return;
            step = new Random().Next(12);
            if (step == 8)
            {
                step++;
            }
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
        }else if (step == 11 & Energy > 2 & eye.Contains(0))
        {
            long loseEnergy = -Math.Abs(Energy / 2);
            UpdateEnergy(loseEnergy);
            Reproduction(x, y, -loseEnergy, bots, Brain, Color, IsStep, session);
        }else
        {
            UpdateEnergy(-1);
        }
        if (Energy <= 0)
        {
            Death(x, y, bots);
        }
    }

    private bool Attack(int xAttacking, int yAttacking, int xDefensive, int yDefensive,  Bot[,] bots)
    {
        long energyA = bots[xAttacking, yAttacking].Energy;
        long energyD = bots[xDefensive, yDefensive].Energy;
        bots[xDefensive, yDefensive].UpdateEnergy(-Math.Abs((long) Math.Round(energyA / 0.5)));
        bots[xAttacking, yAttacking].UpdateEnergy(-Math.Abs((long) Math.Round(energyD / 3.0)));
        if (bots[xDefensive, yDefensive].Energy <= 0)
        {
            bots[xAttacking, yAttacking].UpdateEnergy((long) Math.Abs(Math.Round(energyD / 1.0)), true);
            bots[xDefensive, yDefensive].Death(xDefensive, yDefensive, bots);
            return true;
        }
        return false;
    }

    private int[] Move(int step, int x, int y, int[] eye,  Bot[,] bots)
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
        catch (Exception) { // ignored
        }
        try
        {
            if (step == 1 & bots[x, y - 1] is null)
            {
                bots[x, y - 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x, y - 1};
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (step == 2 & bots[x + 1, y - 1] is null)
            {
                bots[x + 1, y - 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x + 1, y - 1};
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (step == 3 & bots[x + 1, y] is null)
            {
                bots[x + 1, y] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x + 1, y};
            }
        }
        catch (Exception){ // ignored
        }
        try
        {
            if (step == 4 & bots[x + 1, y + 1] is null)
            {
                bots[x + 1, y + 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x + 1, y + 1};
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (step == 5 & bots[x, y + 1] is null)
            {
                bots[x, y + 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x, y + 1};
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (step == 6 & bots[x - 1, y + 1] is null)
            {
                bots[x - 1, y + 1] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x - 1, y + 1};
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (step == 7 & bots[x - 1, y] is null)
            {
                bots[x - 1, y] = bots[x, y];
                bots[x, y] = null;
                return new int[] {x - 1, y};
            }
        }
        catch (Exception) { // ignored
        }

        UpdateEnergy(-10);
        return new int[] {x, y};
    }

    private void Generation(int x, int y, int[,] map)
    {
        UpdateEnergy((long) (map[x, y]));
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

    public void UpdateEnergy(long energyLoss, bool isAttack = false)
    {
        if (this.Energy >= 200 & energyLoss > 0 & !isAttack) {
            isGenerator = 0;
            return;
        }
        if (this.Energy <= 100) {
            isGenerator = 1;
        }
        if (isGenerator == 0 & energyLoss > 0 & !isAttack) {
            return;
        }
        if (isAttack)
        {
            if (this.Energy >= 500 & energyLoss > 0) {
                isGenerator = 0;
                return;
            }
            if (this.Energy <= 250) {
                isGenerator = 1;
            }
            if (isGenerator == 0 & energyLoss > 0) {
                return;
            }
        }
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

    private void Reproduction(int x, int y, long energy, Bot[,] bots, Perceptron brain, string color, int isStep, string session)
    {
        if (energy <= 0)
        {
            bots[x, y].UpdateEnergy(energy - 50);
            return;
        }
        Random random = new Random();
        int mutationProbability = random.Next(100);
        bool isMutation = false;
        if (mutationProbability <= 5)
        {
            brain = brain.MakePerceptron();
            isMutation = true;
            string[] rgb = color.Split(", ");
            int[] rgbInt = new int[3];
            for (int i = 0; i < 3; i++)
            {
                rgbInt[i] = int.Parse(rgb[i]);
            }
            if (rgbInt[0] < 250 & rgbInt[2] == 0)
            {
                rgbInt[0] += random.Next(50) + 100;
                if (rgbInt[0] > 254) { rgbInt[0] = 250; }
            } else if (rgbInt[1] > 0 & rgbInt[2] == 0)
            {
                rgbInt[1] -= random.Next(50) + 100;
                if (rgbInt[1] < 0) { rgbInt[1] = 0; }
            }else if (rgbInt[2] < 250 & rgbInt[1] == 0)
            {
                rgbInt[2] += random.Next(50) + 100;
                if (rgbInt[2] > 254) { rgbInt[2] = 250; }
            }else if (rgbInt[0] > 0)
            {
                rgbInt[0] -= random.Next(50) + 100;
                if (rgbInt[0] < 0) { rgbInt[0] = 0; }
            }else if (rgbInt[1] < 250)
            {
                rgbInt[1] += random.Next(50) + 100;
                if (rgbInt[1] > 254) { rgbInt[1] = 250; }
            }else if (rgbInt[2] > 0)
            {
                rgbInt[2] -= random.Next(50) + 100;
                if (rgbInt[2] < 0) { rgbInt[2] = 0; }
            }else
            {
                rgbInt[0] = random.Next(250);
                rgbInt[1] = 250;
                rgbInt[2] = random.Next(250);
            }
            color = rgbInt[0] + ", " + rgbInt[1] + ", " + rgbInt[2];
        }
        try
        {
            if (bots[x - 1, y - 1] is null)
            {
                bots[x - 1, y - 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (bots[x, y - 1] is null)
            {
                bots[x, y - 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (bots[x + 1, y - 1] is null)
            {
                bots[x + 1, y - 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (bots[x + 1, y] is null)
            {
                bots[x + 1, y] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (bots[x + 1, y + 1] is null)
            {
                bots[x + 1, y + 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (bots[x, y + 1] is null)
            {
                bots[x, y + 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (bots[x - 1, y + 1] is null)
            {
                bots[x - 1, y + 1] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        try
        {
            if (bots[x - 1, y] is null)
            {
                bots[x - 1, y] = new Bot(energy, brain, color, isStep);
                if (isMutation)
                {
                    AjaxController.Maps[session].AddType(brain);
                }
                return;
            }
        }
        catch (Exception) { // ignored
        }
        bots[x, y].UpdateEnergy(energy - 50);
    }
}