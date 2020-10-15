using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class MapManager : MonoBehaviour
{
    public EventOutOfSpace OutOfSpaceHasHappened;

    //todo можно сделать метод для более явного создания вложенности дочерних GO 

    public GameObject prefabHexagon;

    //игровой объект где распололагаются уровни с секторами
    public GameObject Map;

    public GameObject ColliderOutBeyond;

    public int countLevels = 5;
    public float _heightFloor = -10f;

    private BoxCollider _boxCollider;

    //Размер 6ти гранника для подмены размеров
    private SizeF _sizeHexagon;
    private ControllerBeyondSpace _controllerBeyondSpace;

    private int countCellX = 10;
    private int countCellY = 10;

    private float SizeX => countCellX * _sizeHexagon.Width;
    private float SizeY => countCellY * _sizeHexagon.Height;

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

    private void Awake()
    {
        var bounds = prefabHexagon.GetComponent<Renderer>().bounds;
        _sizeHexagon = new SizeF(bounds.size.x, bounds.size.z);
        _boxCollider = ColliderOutBeyond.GetComponent<BoxCollider>();
        _controllerBeyondSpace = ColliderOutBeyond.GetComponent<ControllerBeyondSpace>();
    }

    // Start is called before the first frame update
    void Start()
    {
        var posYCollider = -(((countLevels) * _heightFloor) - _heightFloor * 0.8f);
        _boxCollider.transform.position = new Vector3(SizeX / 2f, posYCollider, SizeY / 2f);
        _boxCollider.size = new Vector3(SizeX * 2f, 1, SizeY * 2);
        _controllerBeyondSpace.TriggerEnterHasHappened += TriggerEnterChanged;
    }


    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        for (int i = 0; i < countLevels; i++)
        {
            Gizmos.DrawCube(new Vector3(SizeX / 2f, i * -_heightFloor, SizeY / 2f), new Vector3(SizeX, 2, SizeY));
        }
    }

    public void SpawnMap()
    {
        for (int i = 0; i < countLevels; i++)
            GenerateStorey(i, countCellX, countCellY, Map);
    }

    public void RemoveMap()
    {
        for (int i = 0; i < Map.transform.childCount; i++)
        {
            Destroy(Map.transform.GetChild(i).gameObject);
        }
    }

    public Vector3[] SpawnCells(int count)
    {
        var positionsSpawn = new Vector3[count];

        var sizeGrid = Mathf.Ceil(Mathf.Sqrt(count));

        var steepX = SizeX / sizeGrid;
        var steepY = SizeY / sizeGrid;

        for (int i = 0; i < count; i++)
        {
            var x = (int) (i / sizeGrid);
            var y = i % sizeGrid;
            var position = new Vector3(x * steepX, 10, y * steepY);
            var cell = FactoryMethodCreateCell(prefabHexagon, position, 0);
            cell.name = $"Player X{x}Y{y}";
            cell.transform.parent = this.Map.transform;
            positionsSpawn[i] = position;
        }

        return positionsSpawn;
    }

    private void CreateCell(float indexX, float indexY, int level, GameObject prefab, GameObject levelObject)
    {
        /*
         * Y /|\
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

    private void GenerateStorey(int level, int countX, int countY, GameObject map)
    {
        var levelGO = new GameObject("Level" + level);
        levelGO.transform.parent = map.transform;
        for (int x = 0; x < countX; x++)
        {
            for (int y = 0; y < countY; y++)
            {
                CreateCell(x, y, level, prefabHexagon, levelGO);
            }
        }
    }

    private void TriggerEnterChanged(Collider collider)
    {
        OutOfSpaceHasHappened?.Invoke(collider);
    }
}

public delegate void EventOutOfSpace(Collider collider);