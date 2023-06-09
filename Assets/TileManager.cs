using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private int[][] map;
    GameObject[][] allTiles;
    [SerializeField] float CELLSIZE;
    public static float GLOBAL_CELLSIZE;
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] private GameObject CellPreFab;
    private Vector2 startingPoint;
    
    // Start is called before the first frame update
    
    
    
    void Start()
    {
        GLOBAL_CELLSIZE = CELLSIZE;
        startingPoint = transform.position;
        InitMap(); 
    }

    void InitMap()
    {
        map = new int[height][];
        allTiles = new GameObject[height][];
        Vector2 currentCellPos = startingPoint;
        for (int i = 0; i < height; i++)
        {
            map[i] = new int[width];
            allTiles[i] = new GameObject[width];
            for (int j = 0; j < width; j++)
            {
                map[i][j] = 1;
                GameObject newGameObject = Instantiate(CellPreFab, currentCellPos, Quaternion.identity);
                MyTile temp = newGameObject.GetComponent<MyTile>();
                temp.typeOfCell = map[i][j];
                temp.Init();
                allTiles[i][j] = newGameObject;
                
                currentCellPos.x += CELLSIZE;
            }
            currentCellPos.x = startingPoint.x;
            currentCellPos.y -= CELLSIZE;
        }
    }
    
    
    
    // Update is called once per frame

}
