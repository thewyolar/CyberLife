using CyberLife.Neuronet;

namespace CyberLife.Models;

public class Map : MapModel
{
    public Bot[,] Bots;
    public IList<Perceptron> BotTypes = new List<Perceptron>();
    private IList<long> BeforeAllEnergyType = new List<long>();
    private IList<long> BeforePopulationType = new List<long>();
    private int Circle = 0;
    public Map()
    {
        
    }

    public Map(int q)
    {
        int n = 55;
        int m = 55;
        MapTypes = new int[n, m];
        Bots = new Bot[n, m];
        for (int i = 0; i < n; i++)
        {
            Random random = new Random();
            for (int j = 0; j < m; j++)
            {
                bool hereBot = random.Next(100) > 98;
                MapTypes[i, j] = (int) MapType.TEMPERATE;
                if (hereBot)
                {
                    Bots[i, j] = new Bot(random.Next(250) + ", " + random.Next(250) +", " + random.Next(250));
                    AddType(Bots[i, j].Brain);
                }
            }
        }
    }

    public void Work()
    {
        int x = MapTypes.GetLength(0);
        int y = MapTypes.GetLength(1);
        for (int j = 0; j < BotTypes.Count; j++)
        {
            if (BotTypes[j].Population <= 0)
            {
                BotTypes.RemoveAt(j);
                BeforePopulationType.RemoveAt(j);
                BeforeAllEnergyType.RemoveAt(j);
            }
        }
        for (int j = 0; j < BotTypes.Count; j++)
        {
            BeforePopulationType[j] = BotTypes[j].Population;
            BeforeAllEnergyType[j] = BotTypes[j].AllEnergy;
        }
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (Bots[i, j] is not null)
                {
                    int[] eye = new int[8];
                    try
                    {
                        if (Bots[i - 1, j - 1] is null) eye[0] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i - 1, j - 1].Color)) eye[0] = 1;
                        else eye[0] = 2;
                    }
                    catch (Exception ignore) { eye[0] = -1; }
                    try
                    {
                        if (Bots[i, j - 1] is null) eye[1] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i, j - 1].Color)) eye[1] = 1;
                        else eye[1] = 2;
                    }
                    catch (Exception ignore) { eye[1] = -1; }
                    try
                    {
                        if (Bots[i + 1, j - 1] is null) eye[2] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i + 1, j + 1].Color)) eye[2] = 1;
                        else eye[2] = 2;
                    }
                    catch (Exception ignore) { eye[2] = -1; }
                    try
                    {
                        if (Bots[i + 1, j] is null) eye[3] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i + 1, j].Color)) eye[3] = 1;
                        else eye[3] = 2;
                    }
                    catch (Exception ignore) { eye[3] = -1; }
                    try
                    {
                        if (Bots[i + 1, j + 1] is null) eye[4] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i + 1, j - 1].Color)) eye[4] = 1;
                        else eye[4] = 2;
                    }
                    catch (Exception ignore) { eye[4] = -1; }
                    try
                    {
                        if (Bots[i, j + 1] is null) eye[5] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i, j - 1].Color)) eye[5] = 1;
                        else eye[5] = 2;
                    }
                    catch (Exception ignore) { eye[5] = -1; }
                    try
                    {
                        if (Bots[i - 1, j + 1] is null) eye[6] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i - 1, j + 1].Color)) eye[6] = 1;
                        else eye[6] = 2;
                    }
                    catch (Exception ignore) { eye[6] = -1; }
                    try
                    {
                        if (Bots[i - 1, j] is null) eye[7] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i - 1, j].Color)) eye[7] = 1;
                        else eye[7] = 2;
                    }
                    catch (Exception ignore) { eye[7] = -1; }
                    if (Bots[i, j].IsStep == 0 & Circle == 0)
                    {
                        Bots[i, j].IsStep++;
                        Bots[i, j].ActivateBrain(eye, i, j, Bots, MapTypes);
                    }else if (Bots[i, j].IsStep == 1 & Circle == 1)
                    {
                        Bots[i, j].IsStep--;
                        Bots[i, j].ActivateBrain(eye, i, j, Bots, MapTypes);
                    }
                }
            }
        }

        if (Circle == 0)
        {
            Circle++;
        }
        else
        {
            Circle--;
        }
        
        for (int i = 0; i < BotTypes.Count; i++)
        {
            BotTypes[i].BackPropagation(BotTypes[i].AllEnergy- BeforeAllEnergyType[i], BotTypes[i].Population - BeforePopulationType[i]);
        }
    }


    public void AddType(Perceptron perceptron)
    {
        BotTypes.Add(perceptron);
        BeforePopulationType.Add(perceptron.Population);
        BeforeAllEnergyType.Add(perceptron.AllEnergy);
    }
    

}