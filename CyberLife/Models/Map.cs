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

public class Map: BaseModel
{
    public MapType[,] mapTypes;
    public Bot[,] bots;
    public IList<Perceptron> perceptronType = new List<Perceptron>();
    public IList<double> beforeAllEnergyType = new List<double>();
    public IList<long> beforePerceptronType = new List<long>();
    public int circle = 0;
    public Map()
    {
        
    }

    public Map(int q)
    {
        int n = 50;
        mapTypes = new MapType[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                mapTypes[i, j] = MapType.TEMPERATE;
            }
        }

        bots = new Bot[n, n];
        bots[0, 0] = new Bot();
        addType(bots[0, 0].brain);
    }

    public void work()
    {
        int x = mapTypes.GetLength(0);
        int iii = 0;
        while (iii < 1)
        {
            for (int j = 0; j < perceptronType.Count; j++)
            {
                if (perceptronType[j].population <= 0)
                {
                    perceptronType.RemoveAt(j);
                    beforePerceptronType.RemoveAt(j);
                    beforeAllEnergyType.RemoveAt(j);
                }
            }
            for (int j = 0; j < perceptronType.Count; j++)
            {
                beforePerceptronType[j] = perceptronType[j].population;
                beforeAllEnergyType[j] = perceptronType[j].allEnergy;
            }
            iii++;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (bots[i, j] is not null)
                    {
                        int[] eye = new int[8];
                        try
                        {
                            if (bots[i - 1, j - 1] is null) eye[0] = 0;
                            else if (bots[i, j].color.Equals(bots[i - 1, j - 1])) eye[0] = 1;
                            else eye[0] = 2;
                        }
                        catch (Exception ignore) { eye[0] = -1; }
                        try
                        {
                            if (bots[i, j - 1] is null) eye[1] = 0;
                            else if (bots[i, j].color.Equals(bots[i, j - 1])) eye[1] = 1;
                            else eye[1] = 2;
                        }
                        catch (Exception ignore) { eye[1] = -1; }
                        try
                        {
                            if (bots[i + 1, j + 1] is null) eye[2] = 0;
                            else if (bots[i, j].color.Equals(bots[i + 1, j + 1])) eye[2] = 1;
                            else eye[2] = 2;
                        }
                        catch (Exception ignore) { eye[2] = -1; }
                        try
                        {
                            if (bots[i + 1, j] is null) eye[3] = 0;
                            else if (bots[i, j].color.Equals(bots[i + 1, j])) eye[3] = 1;
                            else eye[3] = 2;
                        }
                        catch (Exception ignore) { eye[3] = -1; }
                        try
                        {
                            if (bots[i + 1, j - 1] is null) eye[4] = 0;
                            else if (bots[i, j].color.Equals(bots[i + 1, j - 1])) eye[4] = 1;
                            else eye[4] = 2;
                        }
                        catch (Exception ignore) { eye[4] = -1; }
                        try
                        {
                            if (bots[i, j - 1] is null) eye[5] = 0;
                            else if (bots[i, j].color.Equals(bots[i, j - 1])) eye[5] = 1;
                            else eye[5] = 2;
                        }
                        catch (Exception ignore) { eye[5] = -1; }
                        try
                        {
                            if (bots[i - 1, j + 1] is null) eye[6] = 0;
                            else if (bots[i, j].color.Equals(bots[i - 1, j + 1])) eye[6] = 1;
                            else eye[6] = 2;
                        }
                        catch (Exception ignore) { eye[6] = -1; }
                        try
                        {
                            if (bots[i - 1, j] is null) eye[7] = 0;
                            else if (bots[i, j].color.Equals(bots[i - 1, j])) eye[7] = 1;
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
            
            for (int i = 0; i < perceptronType.Count; i++)
            {
                perceptronType[i].BackPropagation(perceptronType[i].allEnergy- beforeAllEnergyType[i], perceptronType[i].population - beforePerceptronType[i]);
            }
            
            
            
        }
    }


    public void addType(Perceptron perceptron)
    {
        perceptronType.Add(perceptron);
        beforePerceptronType.Add(perceptron.population);
        beforeAllEnergyType.Add(perceptron.allEnergy);
    }
    

}