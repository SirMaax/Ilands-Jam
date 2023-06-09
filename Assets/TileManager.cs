using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    private int[][] map;
    MyTile[][] allTiles;
    [SerializeField] float CELLSIZE;
    public static float GLOBAL_CELLSIZE;
    public static float GLOBAL_HEIGHT;
    public static float GLOBAL_WIDTH;
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] private GameObject CellPreFab;
    private Vector2 startingPoint;

    private BuildingClass _buildingClass;
    // Start is called before the first frame update
    
    
    
    void Start()
    {
        _buildingClass = GameObject.FindWithTag("BuildingClass").GetComponent<BuildingClass>();
        GLOBAL_CELLSIZE = CELLSIZE;
        GLOBAL_WIDTH = width;
        GLOBAL_HEIGHT = height;
        startingPoint = transform.position;
        InitMap(); 
    }

    void InitMap()
    {
        map = new int[height][];
        allTiles = new MyTile[height][];
        Vector2 currentCellPos = startingPoint;
        for (int i = 0; i < height; i++)
        {
            map[i] = new int[width];
            allTiles[i] = new MyTile[width];
            for (int j = 0; j < width; j++)
            {
                map[i][j] = 1;
                GameObject newGameObject = Instantiate(CellPreFab, currentCellPos, Quaternion.identity);
                MyTile temp = newGameObject.GetComponent<MyTile>();
                temp.typeOfCell = map[i][j];
                temp.position = new Vector2(i, j);
                temp.Init();
                allTiles[i][j] = temp;
                
                currentCellPos.x += CELLSIZE;
            }
            currentCellPos.x = startingPoint.x;
            currentCellPos.y -= CELLSIZE;
        }
    }

    public bool TileAvailable(int buldingId, Vector2 position)
    {
        int xCoord = (int) position.x;
        int yCoord = (int) position.y;
        //Exists building there
        if (allTiles[xCoord][yCoord].isBuilding) return false;

        //If river or mountain exists there
        if (allTiles[xCoord][yCoord].isBlocked) return false;
        
        //If resource and fits to resource TODO adjust ResourceBuildingMethod
        if (allTiles[xCoord][yCoord].isResource && _buildingClass.ResourceBuilding(buldingId))
        {
            if (_buildingClass.CanMineResource(buldingId, allTiles[xCoord][yCoord].typeOfCell)) return true;
        }
        
        return true;
    }
    
    // Update is called once per frame

}
