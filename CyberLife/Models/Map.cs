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

    public Map()
    {
        
    }

    public Map(int q)
    {
        int n = 7;
        mapTypes = new MapType[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                mapTypes[i, j] = MapType.TEMPERATE;
            }
        }

        bots = new Bot[n, n];
        bots[6, 6] = new Bot();
    }

    public void work()
    {
        int x = mapTypes.GetLength(0);
        int iii = 0;
        while (iii < 1)
        {
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
                        bots[i, j].activateBrain(eye, i, j, bots, mapTypes);
                        // bots[i, j].move(i , j, bots);
                    }
                }
            }
        }
    }
    
    

}