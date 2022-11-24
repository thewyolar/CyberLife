using CyberLife.Neuronet;

namespace CyberLife.Models;

public enum MapType
{
    DESERT = 2,
    WATER = 0,
    TROPIC = 7,
    TEMPERATE = 6,
    TAIGA = 3,
    TUNDRA = 1
    
}

public class Map
{
    public MapType[,] mapTypes;
    public Bot[,] bots;
    public IList<Perceptron> botTypes = new List<Perceptron>();
    private IList<long> beforeAllEnergyType = new List<long>();
    private IList<long> beforePopulationType = new List<long>();
    private int circle = 0;
    public Map()
    {
        
    }

    public Map(int q)
    {
        int n = 25;
        int m = 25;
        mapTypes = new MapType[n, m];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                mapTypes[i, j] = MapType.TEMPERATE;
            }
        }

        bots = new Bot[n, m];
        bots[0, 0] = new Bot();
        addType(bots[0, 0].brain);
    }

    public void work()
    {
        int x = mapTypes.GetLength(0);
        int y = mapTypes.GetLength(1);
        int iii = 0;
        while (iii < 1)
        {
            for (int j = 0; j < botTypes.Count; j++)
            {
                if (botTypes[j].population <= 0)
                {
                    botTypes.RemoveAt(j);
                    beforePopulationType.RemoveAt(j);
                    beforeAllEnergyType.RemoveAt(j);
                }
            }
            for (int j = 0; j < botTypes.Count; j++)
            {
                beforePopulationType[j] = botTypes[j].population;
                beforeAllEnergyType[j] = botTypes[j].allEnergy;
            }
            iii++;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (bots[i, j] is not null)
                    {
                        int[] eye = new int[8];
                        try
                        {
                            if (bots[i - 1, j - 1] is null) eye[0] = 0;
                            else if (bots[i, j].color.Equals(bots[i - 1, j - 1].color)) eye[0] = 1;
                            else eye[0] = 2;
                        }
                        catch (Exception ignore) { eye[0] = -1; }
                        try
                        {
                            if (bots[i, j - 1] is null) eye[1] = 0;
                            else if (bots[i, j].color.Equals(bots[i, j - 1].color)) eye[1] = 1;
                            else eye[1] = 2;
                        }
                        catch (Exception ignore) { eye[1] = -1; }
                        try
                        {
                            if (bots[i + 1, j - 1] is null) eye[2] = 0;
                            else if (bots[i, j].color.Equals(bots[i + 1, j + 1].color)) eye[2] = 1;
                            else eye[2] = 2;
                        }
                        catch (Exception ignore) { eye[2] = -1; }
                        try
                        {
                            if (bots[i + 1, j] is null) eye[3] = 0;
                            else if (bots[i, j].color.Equals(bots[i + 1, j].color)) eye[3] = 1;
                            else eye[3] = 2;
                        }
                        catch (Exception ignore) { eye[3] = -1; }
                        try
                        {
                            if (bots[i + 1, j + 1] is null) eye[4] = 0;
                            else if (bots[i, j].color.Equals(bots[i + 1, j - 1].color)) eye[4] = 1;
                            else eye[4] = 2;
                        }
                        catch (Exception ignore) { eye[4] = -1; }
                        try
                        {
                            if (bots[i, j + 1] is null) eye[5] = 0;
                            else if (bots[i, j].color.Equals(bots[i, j - 1].color)) eye[5] = 1;
                            else eye[5] = 2;
                        }
                        catch (Exception ignore) { eye[5] = -1; }
                        try
                        {
                            if (bots[i - 1, j + 1] is null) eye[6] = 0;
                            else if (bots[i, j].color.Equals(bots[i - 1, j + 1].color)) eye[6] = 1;
                            else eye[6] = 2;
                        }
                        catch (Exception ignore) { eye[6] = -1; }
                        try
                        {
                            if (bots[i - 1, j] is null) eye[7] = 0;
                            else if (bots[i, j].color.Equals(bots[i - 1, j].color)) eye[7] = 1;
                            else eye[7] = 2;
                        }
                        catch (Exception ignore) { eye[7] = -1; }
                        if (bots[i, j].isStep == 0 & circle == 0)
                        {
                            bots[i, j].isStep++;
                            bots[i, j].activateBrain(eye, i, j, bots, mapTypes);
                        }else if (bots[i, j].isStep == 1 & circle == 1)
                        {
                            bots[i, j].isStep--;
                            bots[i, j].activateBrain(eye, i, j, bots, mapTypes);
                        }
                    }
                }
            }

            if (circle == 0)
            {
                circle++;
            }
            else
            {
                circle--;
            }
            
            for (int i = 0; i < botTypes.Count; i++)
            {
                botTypes[i].BackPropagation(botTypes[i].allEnergy- beforeAllEnergyType[i], botTypes[i].population - beforePopulationType[i]);
            }
            
            
            
        }
    }


    public void addType(Perceptron perceptron)
    {
        botTypes.Add(perceptron);
        beforePopulationType.Add(perceptron.population);
        beforeAllEnergyType.Add(perceptron.allEnergy);
    }
    

}