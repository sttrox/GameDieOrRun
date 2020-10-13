using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class ManagerMap : MonoBehaviour
{
    //todo можно сделать метод для более явного создания вложенности дочерних GO 

    public GameObject PrefabHexagon;

    //игровой объект где распололагаются уровни с секторами
    public GameObject Map;

    public int countLevels = 5;
    public float _heightFloor = -10f;

    //Размер 6ти гранника для подмены размеров
    private SizeF _sizeHexagon;


    private int sizeX = 10;
    private int sizeY = 10;

    private readonly Color[] _paletteColor = new Color[]
    {
        CreateColor(235, 52, 52), //red
        CreateColor(235, 122, 52), //orange
        CreateColor(235, 220, 52), //yellow
        CreateColor(140, 235, 52), //green?
        CreateColor(52, 235, 67), //green??
        CreateColor(52, 235, 180), //aqua
        CreateColor(52, 192, 235), //blue
        CreateColor(180, 52, 235), //purple 
        CreateColor(235, 52, 98), //oh, my eyes!
    };

    private static Color CreateColor(int r, int g, int b)
        => new Color(255f / r, 255f / g, 255f / b);

// Start is called before the first frame update
    void Start()
    {
        var bounds = PrefabHexagon.GetComponent<Renderer>().bounds;
        _sizeHexagon = new SizeF(bounds.size.x, bounds.size.z);

        for (int i = 0; i < countLevels; i++)
        {
            GenerateStorey(i, sizeX, sizeY, Map);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GenerateStorey(int level, int countX, int countY, GameObject map)
    {
        var levelGO = new GameObject("Level" + level);
        levelGO.transform.parent = map.transform;
        for (int x = 0; x < countX; x++)
        {
            for (int y = 0; y < countY; y++)
            {
                CreateCell(x, y, level, PrefabHexagon, levelGO);
            }
        }
    }

    private void CreateCell(float indexX, float indexY, int level, GameObject prefab, GameObject levelObject)
    {
        /*
         * X /|\
         *    | X • X
         *    | • X •
         *    | X • X
         *    | • X •
         *    ------------> X
         */
        //смещение по x для устранения промежутков вызванных формой шестигранника(линейное увеличение значения)   
        var offsetX = indexX * (0.25f) * _sizeHexagon.Width;
        //Смещение по Y для сдвига четной линии для состыковки шестигранника(прибавляется половина размера чанки)(фиксированное значение)
        var offsetY = indexX % 2 == 0 ? 0.5f : 0f;
        var posX = indexX * _sizeHexagon.Width - offsetX;
        var posY = (indexY) * _sizeHexagon.Height + _sizeHexagon.Height * offsetY;

        //вычисляем конечное положение y(z) поменяв знак на отрицательный для построения этажей вниз 
        var y = -(level * _heightFloor);
        //Инициализация ячекйи
        var cell = FactoryMethodCreateCell(prefab, new Vector3(posX, y, posY), level);
        //для отладки
#if DEBUG
        cell.name = "X" + indexX + "Y" + indexY;
#endif
        cell.transform.parent = levelObject.transform;
    }

    private GameObject FactoryMethodCreateCell(GameObject prefab, Vector3 vector3, int level)
    {
        var instance = Instantiate(prefab, vector3, Quaternion.identity);
        var colorController = prefab.GetComponent<ControllerColor>();
        colorController.baseColor = _paletteColor[level];
        return instance;
    }
}