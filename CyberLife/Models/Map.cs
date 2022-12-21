using CyberLife.Neuronet;

namespace CyberLife.Models;

public enum BiomType
{
    DESERT = 2,
    WATER = 0,
    TROPIC = 7,
    TEMPERATE = 6,
    TAIGA = 3,
    TUNDRA = 1
    
}
public class Map : MapModel
{
    public Bot[,] Bots;
    public IList<Perceptron> BotTypes = new List<Perceptron>();
    private IList<long> BeforeAllEnergyType = new List<long>();
    private IList<long> BeforePopulationType = new List<long>();
    private int Circle = 0;
    private Dictionary<int, string> ColorMapInt = new Dictionary<int, string>
    {
        {2, "163, 116, 21"},
        {0, "12, 86, 204"},
        {7, "10, 84, 6"},
        {6, "32, 168, 27"},
        {3, "6, 191, 102"},
        {1, "6, 191, 191"}
    };

    public string[,] ColorMap;
    public Map()
    {
        CreateMap(width: 78, height: 40, widthBiome: 30, sizeBiome: 300);
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
                        else if (Bots[i, j].Color.Equals(Bots[i + 1, j - 1].Color)) eye[2] = 1;
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
                        else if (Bots[i, j].Color.Equals(Bots[i + 1, j + 1].Color)) eye[4] = 1;
                        else eye[4] = 2;
                    }
                    catch (Exception ignore) { eye[4] = -1; }
                    try
                    {
                        if (Bots[i, j + 1] is null) eye[5] = 0;
                        else if (Bots[i, j].Color.Equals(Bots[i, j + 1].Color)) eye[5] = 1;
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

    private void CreateMap(int width = 10, int height = 10, int botSpawnChance = 50, int widthBiome = 5, int sizeBiome = 50)
    {
        int n = width;
        int m = height;
        MapTypes = new int[n, m];
        Bots = new Bot[n, m];
        ColorMap = new string[n, m];
        Random random = new Random();
        IList<int[]> bioms;
       
        // Цикл генирации биомов
        bioms = BiomeGeneration(MapTypes.Length, n, m, sizeBiome, widthBiome);
        // summ должна ровняться общему количеству клеток на карте (MapTypes.Length)
        int summ = 0;
        for (int i = 0; i < bioms.Count; i++)
        {
            summ += bioms[i][1];
        }
        Console.WriteLine(summ + " == " + MapTypes.Length);
        // k = biom[i][1]; - количество оставшихся клеток для размещения на карте
        // q biom[i][0]; - ширина биома
        // x и y - итерация по карте
        int k = 0;
        int q = 0;
        int x = 0;
        int y = 0;
        // i - количество биомов
        for (int i = 0; i < bioms.Count; i++)
        {
            Console.WriteLine(i);
            k = bioms[i][1];
            x = 0;
            for (; x <= MapTypes.GetLength(0) & k != 0; x++)
            {
                // если k == 0 значит все клетки в биома размещены на карте
                if (k == 0)
                {
                    break;
                }
                if (x >= MapTypes.GetLength(0))
                {
                    x = 0;
                }
                q = bioms[i][0];
                y = 0;
                for (;y < MapTypes.GetLength(1); summ--, q--, k--, y++)
                {
                    // если k(все клетки биома) или q(ширина биома) == 0 -> выйти
                    if (k == 0 || q == 0)
                    { 
                        break;
                    }
                    try
                    {
                        // если на клектке уже есть биом итерируем цикл и проверяем следующию
                        if (ColorMap[x, y] is null)
                        {
                            MapTypes[x, y] = bioms[i][2];
                            ColorMap[x, y] = ColorMapInt[bioms[i][2]];
                        }else
                        {
                            // итерируется только y 
                            if (x == MapTypes.GetLength(0) - 1)
                            {
                                q++;
                                summ++;
                                k++;
                                x = 0;
                                continue;
                            }
                            // Ни чего не итерируется. x++
                            else
                            {
                                x++;
                                y--;
                                q++;
                                k++;
                                summ++;
                                continue;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                    bool hereBot = random.Next(100) < botSpawnChance;
                    if (hereBot)
                    {
                        Bots[x, y] = new Bot(random.Next(250) + ", " + "250, " + random.Next(250));
                        AddType(Bots[x, y].Brain);
                    }

                }
            }
        }
        Console.WriteLine("EEE " + summ);
        Console.WriteLine("EEE 2 " + bioms.Count);
    }

    private IList<int[]> BiomeGeneration(int mapLength, int x, int y, int sizeBiome, int widthBiome)
    {
        IList<int[]> bioms = new List<int[]>();
        Random random = new Random();
        int k = 0;
        // Цикл генирации биомов
        while (mapLength > 0){
            // billomSizeM общиее количество клеток в биме
            // dice какой биом выбрать
            // biome - биом
            int billomSizeY = random.Next(y);
            int dice = random.Next(100);
            int biome = 0;
            if (dice < 5){ 
                biome = (int)BiomType.WATER;
            }
            else if (dice < 10){
                biome = (int)BiomType.TUNDRA;
            }
            else if (dice < 15){
                biome = (int)BiomType.DESERT;
            }
            else if (dice < 20){
                biome = (int)BiomType.TAIGA;
            }
            else if (dice < 60){
                biome = (int)BiomType.TEMPERATE;
            }
            else if (dice < 100){
                biome = (int)BiomType.TROPIC;
            }
            // Помещяется ли биом в карту
            if (!(mapLength - (billomSizeY + sizeBiome) < 0)){
                // добавление биома в общия массив
                bioms.Add(new []{random.Next(x / 2) + widthBiome, billomSizeY + sizeBiome, biome});
                mapLength -= bioms[k][1];
                k++;
            }
            else{
                // добавление последниго биома в общий массив
                bioms.Add(new []{billomSizeY, billomSizeY + (mapLength - billomSizeY), biome});
                mapLength -= bioms[k][1];
            }
        }
        return bioms;
    }


}