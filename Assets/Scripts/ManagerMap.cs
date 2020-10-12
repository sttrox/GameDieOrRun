using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ManagerMap : MonoBehaviour
{
    public GameObject PrefabHexagon;

    //игровой объект где распололагаются уровни с секторами
    public GameObject Map;
    private SizeF _sizeHexagon;


    private float _heightFloor = 10;

    private int sizeX = 10;

    private int sizeY = 10;

    // Start is called before the first frame update
    void Start()
    {
        var colliderHex = PrefabHexagon.GetComponent<BoxCollider>();
        _sizeHexagon = new SizeF(colliderHex.size.x, colliderHex.size.z);
        //DEBUG
        //_sizeHexagon = new SizeF(0.3f, 0.3f);
        Debug.Log(_sizeHexagon);
        GenerateStorey(1, sizeX, sizeY, Map);
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
        var offsetX = indexX * (0.25f) * _sizeHexagon.Width;
        var offsetY = indexX % 2 == 0 ? 0.5f : 0f;
        var posX = indexX * _sizeHexagon.Width - offsetX;
        var posY = (indexY) * _sizeHexagon.Height + _sizeHexagon.Height * offsetY;
        var cell = Instantiate(prefab, new Vector3(posX, level * _heightFloor, posY), Quaternion.identity);
        cell.name = "X" + indexX + "Y" + indexY;
        cell.transform.parent = levelObject.transform;
    }
}