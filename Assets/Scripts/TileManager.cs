using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    
    [Header("Refs")]
    private BuildingClass _buildingClass;

    [Header("Refs")]
    [SerializeField] private GameObject container;
        
    private int[][] map;
    MyTile[][] allTiles;
    [SerializeField] float CELLSIZE;
    public static float GLOBAL_CELLSIZE;
    public static float GLOBAL_HEIGHT;
    public static float GLOBAL_WIDTH;
    public static Vector2 GLOBAL_GRID_START;
    public static int PylonRange = 4;
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] private GameObject CellPreFab;
    private Vector2 startingPoint;


    [Header("Energy")] private Vector2 mainCore;
    // Start is called before the first frame update

    public void EndTurn()
    {
        CheckEnergy();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                allTiles[i][j].Turn();
            }
        }
        
    }
    
    void Start()
    {
        GLOBAL_GRID_START = transform.position;
        _buildingClass = GameObject.FindWithTag("BuildingClass").GetComponent<BuildingClass>();
        GLOBAL_CELLSIZE = CELLSIZE;
        GLOBAL_WIDTH = width;
        GLOBAL_HEIGHT = height;
        startingPoint = transform.position;
        InitMap(); 
        mainCore = Vector2.zero;
        SetMainCore();
        
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
                map[i][j] = 0;
                if (j == 13 && i == 8) map[i][j] = 11;
                GameObject newGameObject = Instantiate(CellPreFab, currentCellPos, Quaternion.identity);
                newGameObject.transform.SetParent(container.transform);
                MyTile temp = newGameObject.GetComponent<MyTile>();
                temp.typeOfCell = map[i][j];
                temp.position = new Vector2(i, j);
                temp.Init(this);
                
                allTiles[i][j] = temp;
                
                currentCellPos.x += CELLSIZE;
            }
            currentCellPos.x = startingPoint.x;
            currentCellPos.y -= CELLSIZE;
        }
    }

    public bool TileAvailable(int buldingId, Vector2 position)
    {
        if(position.x == -1)return false;
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
    public Vector2 GetRealWorldCoordinates(Vector2 tiles)
    {
        return allTiles[(int)tiles.x][(int)tiles.y].transform.position;

    }

    public void ChangeCell(Vector2 cellPos, int newCellTyp)
    {
        allTiles[(int)cellPos.x][(int)cellPos.y].UpdateTile(newCellTyp);

    }

    public bool IsPoweredPylonInRange(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        if (x > 0) return allTiles[x - 1][y].IsPoweredPylon();
        if (x != width-1) return allTiles[x + 1][y].IsPoweredPylon();
        if (x > 0 && y > 0) return allTiles[x - 1][y-1].IsPoweredPylon();
        if (y > 0) return allTiles[x][y-1].IsPoweredPylon();
        if (x != width-1 && y > 0) return allTiles[x + 1][y-1].IsPoweredPylon();
        if (x != width-1 && y != height-1) return allTiles[x + 1][y+1].IsPoweredPylon();
        if (x > 0 && y != height-1) return allTiles[x - 1][y+1].IsPoweredPylon();
        if (y != height-1) return allTiles[x][y+1].IsPoweredPylon();
        return false;
    }

    public void SetMainCore()
    {
        if (mainCore == Vector2.zero)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (allTiles[i][j].typeOfCell == 11)
                    {
                        mainCore = new Vector2(i, j);
                        return;
                    }
                }
            }
        }
    }

    public void CheckEnergy()
    {
        List < MyTile > allPylons= new List<MyTile>();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (allTiles[i][j].typeOfCell == 15)
                {
                    allPylons.Add(allTiles[i][j]);
                }
            }
        }

        bool changed = true;
        while (changed == true)
        {
            changed = false;
            foreach (var pylon in allPylons)
            {
                
                if ((pylon.position - mainCore).magnitude <= PylonRange &&
                    !pylon.isPowered)
                {
                    pylon.isPowered = true;
                    changed = true;
                }

                if (pylon.isPowered)
                {
                    foreach (var secondayPylon in allPylons)
                    {
                        if (secondayPylon.position == pylon.position) continue;
                        if (secondayPylon.isPowered) continue;
                        if((pylon.position - secondayPylon.position).magnitude <= PylonRange)
                        {
                            secondayPylon.isPowered = true;
                            changed = true;
                        }
                        else
                        {
                            secondayPylon.isPowered = false;
                        }
                    }
                }
            }
        }
    }
}
