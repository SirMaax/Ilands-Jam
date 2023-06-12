using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int idTileManager;
    
    
    [Header("Refs")]
    private BuildingClass _buildingClass;
    [SerializeField] private ResourceManager _resourceManager;
    
    [Header("Refs")]
    [SerializeField] private GameObject container;
        
    private int[][] map;
    MyTile[][] allTiles;
    [SerializeField] float CELLSIZE;
    public static float GLOBAL_CELLSIZE;
    public static float GLOBAL_HEIGHT;
    public static float GLOBAL_WIDTH;
    public static Vector2 GLOBAL_GRID_START;
    public static int PylonRange = 3;
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
        InitMapArray();
        InitMap(); 
        mainCore = Vector2.zero;
        SetMainCore();
        
    }

    void InitMap()
    {
        // map = new int[height][];
        allTiles = new MyTile[height][];
        Vector2 currentCellPos = startingPoint;
        for (int i = 0; i < height; i++)
        {
            // map[i] = new int[width];
            allTiles[i] = new MyTile[width];
            for (int j = 0; j < width; j++)
            {
                // map[i][j] = 1;
                if (j == 7 && i == 4) map[i][j] = 11;
                if (j == 0 && i == 0) map[i][j] = 20;
                GameObject newGameObject = Instantiate(CellPreFab, currentCellPos, Quaternion.identity);
                newGameObject.transform.SetParent(container.transform);
                MyTile temp = newGameObject.GetComponent<MyTile>();
                
                temp.typeOfCell = map[i][j];
                temp.position = new Vector2(i, j);
                temp.Init(this,_resourceManager);
                    
                
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
        MyTile tile = allTiles[xCoord][yCoord];

        if (tile.isBuilding) return false;
        if (tile.isBlocked) return false;
        if (tile.isResource)
        {
            if (tile.typeOfCell == 19 && buldingId == 5) return true;
            if (tile.typeOfCell == 20 && buldingId == 4) return true;
            if (tile.typeOfCell == 21 && buldingId == 6) return true;
            return false;
        }

        if (buldingId == 4 || buldingId == 5 || buldingId == 6) return false;
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


        if (x > 0 && allTiles[x - 1][y].IsPoweredPylon()) return true;
        if (x != height-1   && allTiles[x + 1][y].IsPoweredPylon())return true;
        if (x > 0 && y > 0 && allTiles[x - 1][y-1].IsPoweredPylon())return true;
        if (y > 0  && allTiles[x][y-1].IsPoweredPylon())return true;
        if (x != height-1 && y > 0 &&  allTiles[x + 1][y-1].IsPoweredPylon())return true;
        if (x != height-1 && y != width-1 &&  allTiles[x + 1][y+1].IsPoweredPylon())return true;
        if (x > 0 && y != width-1        &&  allTiles[x - 1][y+1].IsPoweredPylon())return true;
        if (y != width-1                  && allTiles[x][y+1].IsPoweredPylon())return true;
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
                if (allTiles[i][j].typeOfCell == 15 || allTiles[i][j].typeOfCell == 11)
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

        foreach (var pylon in allPylons)
        {
            pylon.UpdateSprite();
        }
    }

    public static bool TileIsInMap(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        
        if (x < 0) return false;
        if (y < 0) return false;
        if (x > GLOBAL_HEIGHT - 1) return false;
        if (y > GLOBAL_WIDTH - 1) return false;

        return true;
    }

    private void InitMapArray()
    {
       
        if (idTileManager == 1)
        {
            map = new int[height][];
            for (int i = 0; i < height; i++)
            {
                map[i] = new int[width];
                for (int j = 0; j < width; j++)
                {
                    map[i][j] = 1;
                    InitHelp(i, j, 0, 0, 19);
                    InitHelp(i, j, 0, 1, 19);
                    InitHelp(i, j, 1, 1, 19);
                    InitHelp(i, j, 0, 1, 19);
                    InitHelp(i, j, 1, 12, 19);
                    InitHelp(i, j, 1, 13, 19);
                    InitHelp(i, j, 2, 13, 19);
                    InitHelp(i, j, 2, 12, 19);
                    InitHelp(i, j, 3, 8, 19);
                    InitHelp(i, j, 5, 2, 19);
                    InitHelp(i, j, 5, 3, 19);
                    InitHelp(i, j, 6, 2, 19);
                    InitHelp(i, j, 6, 3, 19);
                    
                    InitHelp(i, j, 0, 2, 20);
                    InitHelp(i, j, 1, 2, 20);
                    InitHelp(i, j, 2, 2, 20);
                    InitHelp(i, j, 1, 2, 20);
                    InitHelp(i, j, 2, 7, 20);
                    InitHelp(i, j, 2, 8, 20);
                    InitHelp(i, j, 1, 7, 20);
                    InitHelp(i, j, 1, 14, 20);
                    InitHelp(i, j, 8, 6, 20);
                    InitHelp(i, j, 8, 7, 20);
                    InitHelp(i, j, 8, 12, 20);


                    InitHelp(i, j, 2, 0, 21);
                    InitHelp(i, j, 6, 1, 21);
                    InitHelp(i, j, 6, 14, 21);
                    InitHelp(i, j, 6, 13, 21);
                    InitHelp(i, j, 7, 14, 21);

                    InitHelp(i, j, 4, 4, 16);
                    InitHelp(i, j, 4, 3, 16);
                    InitHelp(i, j, 5, 4, 16);
                    InitHelp(i, j, 6, 4, 16);
                    InitHelp(i, j, 7, 4, 16);
    
                    InitHelp(i, j, 0, 4, 17);
                    InitHelp(i, j, 1, 4, 17);
                    InitHelp(i, j, 8, 0, 17);
                    InitHelp(i, j, 8, 1, 17);
                    InitHelp(i, j, 8, 8, 17);
                    InitHelp(i, j, 8, 13, 17);
                    InitHelp(i, j, 8, 14, 17);
                    InitHelp(i, j, 5, 10, 17);
                    InitHelp(i, j, 5, 11, 17);
                    InitHelp(i, j, 4, 10, 17);
                    
                }
            }
        }
        else
        {
            map = new int[height][];
            for (int i = 0; i < height; i++)
            {
                map[i] = new int[width];
                for (int j = 0; j < width; j++)
                {
                    map[i][j] = 1;
                   InitHelp(i,j,0,0,16);
                   InitHelp(i,j,0,1,16);
                   InitHelp(i,j,0,2,16);
                   InitHelp(i,j,1,2,16);
                   InitHelp(i,j,1,3,16);
                   InitHelp(i,j,1,4,16);

                   InitHelp(i,j,2,4,16);
                   InitHelp(i,j,3,4,16);
                   InitHelp(i,j,4,4,16);
                   InitHelp(i,j,0,11,16);
                   InitHelp(i,j,1,11,16);
                   InitHelp(i,j,1,12,16);
                   InitHelp(i,j,1,13,16);
                   InitHelp(i,j,1,14,16);
                   InitHelp(i,j,8,4,16);
                   InitHelp(i,j,8,5,16);
                   
                   InitHelp(i,j,5,1,17);
                   InitHelp(i,j,0,12,17);
                   InitHelp(i,j,0,13,17);
                   InitHelp(i,j,0,14,17);
                   InitHelp(i,j,4,14,17);
                   InitHelp(i,j,4,12,17);
                   InitHelp(i,j,5,14,17);
                   InitHelp(i,j,5,12,17);
                   InitHelp(i,j,5,11,17);
                   InitHelp(i,j,6,11,17);
                   InitHelp(i,j,6,10,17);
                   InitHelp(i,j,7,10,17);
                   InitHelp(i,j,7,9,17);
                   InitHelp(i,j,8,8,17);
                   InitHelp(i,j,8,9,17);
                   
                   InitHelp(i,j,8,11,19);
                   InitHelp(i,j,8,12,19);
                   InitHelp(i,j,8,13,19);
                   InitHelp(i,j,8,14,19);
                   InitHelp(i,j,7,12,19);
                   InitHelp(i,j,7,13,19);
                   InitHelp(i, j, 7, 14, 19);
                   InitHelp(i, j, 7, 8, 19);
                   InitHelp(i, j, 6, 9, 19);
                   InitHelp(i, j, 8, 10, 19);

                   InitHelp(i, j, 7, 11, 19);
                   InitHelp(i, j, 8, 7, 19);

                   InitHelp(i,j,3,0,20);
                   InitHelp(i,j,4,0,20);
                   InitHelp(i,j,5,0,20);
                   InitHelp(i,j,6,0,20);
                   InitHelp(i,j,7,0,20);
                   InitHelp(i,j,4,1,20);
                   InitHelp(i,j,6,1,20);
                   InitHelp(i,j,4,2,20);
                   InitHelp(i,j,6,2,20);
                   InitHelp(i,j,5,2,20);
                   InitHelp(i,j,4,6,20);
                   InitHelp(i,j,0,7,20);
                   InitHelp(i,j,0,8,20);
                }
            }
        }
    }

    public void InitHelp(int i, int j, int x, int y, int building)
    {
        if(x == i && j == y)
        {
            map[i][j] = building;
        }
    }
}
