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
        int n = 30;
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


}