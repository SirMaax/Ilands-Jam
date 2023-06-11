using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsoMapManager : MonoBehaviour
{
    [Header("test")] [SerializeField] private bool test;
    [SerializeField] private Vector2 testVector;
    [SerializeField] private int testRange;

    [Header("Map Stuff")] public BaseTile[][] allTiles;
    private int[][] typeofTiles;
    public static int WIDTH = 8;
    public static int HEIGHT = 8;
    [Header("Refs")] [SerializeField] private GameObject baseTilePrefab;
    [SerializeField] private Vector2 firstTile;
    [SerializeField] private int TILEMAP_START_X;
    [SerializeField] private int TILEMAP_START_y;
    

    [Header("Tiles")] [SerializeField] private TileBase[] prefabTiles;
    private List<Vector2> dontRemoveTileSign;
    [SerializeField] public Tilemap tilemap;

    // Start is called before the first frame update
    public void OwnStart()
    {
        dontRemoveTileSign = new List<Vector2>();
        Quaternion rot = new Quaternion(-0.463689953f, 0.18706049f, 0.323998272f, 0.803134561f);
        allTiles = new BaseTile[HEIGHT][];
        typeofTiles = new int[HEIGHT][];
        for (int i = 0; i < HEIGHT; i++)
        {
            allTiles[i] = new BaseTile[WIDTH];
            typeofTiles[i] = new int[WIDTH];
            for (int j = 0; j < WIDTH; j++)
            {
                Vector2 position = new Vector2(firstTile.x + (j * 0.5f * 2), firstTile.y + (j * 0.25f * 2));

                typeofTiles[i][j] = 1;
                GameObject tempGameObject = Instantiate(baseTilePrefab, position, rot);
                tempGameObject.transform.SetParent(transform);
                BaseTile temp = tempGameObject.GetComponent<BaseTile>();

                temp.Init(new Vector2(i, j), typeofTiles[i][j]);
                allTiles[i][j] = temp;
            }

            firstTile += new Vector2(0.5f * 2, -0.25f * 2);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = false;
            Vector3Int pos = new Vector3Int((int)testVector.x, (int)testVector.y, 3);
            tilemap.SetTile(pos, prefabTiles[3]);
        }
    }

    public Vector2 GetRealWorldCords(Vector2 pos)
    {
        return allTiles[(int)pos.x][(int)pos.y].transform.position;
    }

    public void AddTileAt(int x, int y, int tileId, int layer)
    {
        if (allTiles[x][y].EnemySpawning && tileId != 11) return;
        if (dontRemoveTileSign.Contains(new Vector2(x, y))) return;
        if (tileId == 9)
        {
            dontRemoveTileSign.Add(new Vector2(x,y));
        }
        Vector3Int pos = new Vector3Int(TILEMAP_START_X + y, TILEMAP_START_y - x, layer);
        tilemap.SetTile(pos, prefabTiles[tileId]);
    }

    public void RemoveTileAt(int x, int y, int layer)
    {
        Vector3Int pos = new Vector3Int(TILEMAP_START_X + y, TILEMAP_START_y - x, layer);
        tilemap.SetTile(pos, null);
    }

    public void ShowIfCanMoveTo(Vector2 isogridPos, int range)
    {
        int x = (int)isogridPos.x;
        int y = (int)isogridPos.y;
        allTiles[x][y].isInMoveRange = true;
        List<BaseTile> allMarkedTiles = new List<BaseTile>();
        for (int k = 0; k <= range; k++)
        {
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    if (allTiles[i][j].isInMoveRange)
                    {
                        if (!allMarkedTiles.Contains(allTiles[i][j])) allMarkedTiles.Add(allTiles[i][j]);
                    }
                }
            }

            foreach (var ele in allMarkedTiles)
            {
                if (ele.blocked && ele.position != isogridPos) continue;
                if (ele.position == isogridPos) MarkAllNeighborsOf2((int)ele.position.x, (int)ele.position.y);
                else MarkAllNeighborsOf((int)ele.position.x, (int)ele.position.y);
            }
        }

        foreach (var ele in allMarkedTiles)
        {
            if (ele.position == isogridPos)
            {
                if (!dontRemoveTileSign.Contains(ele.position))
                {
                AddTileAt((int)ele.position.x, (int)ele.position.y, 8, 2);
                continue;
                }
            }

            if (ele.blocked) AddTileAt((int)ele.position.x, (int)ele.position.y, 1, 2);
            else AddTileAt((int)ele.position.x, (int)ele.position.y, 2, 2);
        }
        // RemoveTileAt(x,y,2);
    }


    public void MarkAllNeighborsOf(int i, int j)
    {
        if (i != 0) allTiles[i - 1][j].isInMoveRange = true;
        // else if (i != 0 && allTiles[i - 1][j].blocked) AddTileAt(i - 1, j, 1, 2);
        if (i != 7) allTiles[i + 1][j].isInMoveRange = true;
        // else if (i != 7 && !allTiles[i + 1][j].blocked) AddTileAt(i + 1, j, 1, 2);
        if (j != 0 ) allTiles[i][j - 1].isInMoveRange = true;
        // else if (j != 0 && !allTiles[i][j - 1].blocked) AddTileAt(i, j - 1, 1, 2);
        if (j != 7 ) allTiles[i][j + 1].isInMoveRange = true;
        // else if (j != 7 && !allTiles[i][j + 1].blocked) AddTileAt(i, j + 1, 1, 2);
    }
    public void MarkAllNeighborsOf2(int i, int j)
    {
        if (i != 0 && !allTiles[i - 1][j].blocked) allTiles[i - 1][j].isInMoveRange = true;
        if (i != 7 && !allTiles[i + 1][j].blocked) allTiles[i + 1][j].isInMoveRange = true;
        if (j != 0 && !allTiles[i][j - 1].blocked) allTiles[i][j - 1].isInMoveRange = true;
        if (j != 7 && !allTiles[i][j + 1].blocked) allTiles[i][j + 1].isInMoveRange = true;
    }

    public void RemoveAllSignTiles()
    {
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                if (allTiles[i][j].EnemySpawning) continue;
                if (!dontRemoveTileSign.Contains(new Vector2(i,j)))
                {
                    RemoveTileAt(i, j, 2);
                    allTiles[i][j].isInMoveRange = false;
                    
                }
            }
        }
    }

    public void TileXisNowBlocked(Vector2 isogridPos)
    {
        int x = (int)isogridPos.x;
        int y = (int)isogridPos.y;

        allTiles[x][y].blocked = true;
    }

    public void TileXisNowLongerBlocked(Vector2 isogridPos)
    {
        int x = (int)isogridPos.x;
        int y = (int)isogridPos.y;

        allTiles[x][y].blocked = false;
    }

    public Vector2 CanReach(Vector2 pos, Vector2 targetPos, int range, bool differentMode = false)
    {
        Vector2 result = new Vector2(-1, -1);
        int x = (int)pos.x;
        int y = (int)pos.y;
        List<BaseTile> allVisitedTiles = new List<BaseTile>();
        allVisitedTiles.Add(allTiles[x][y]);
        for (int k = 0; k <= range; k++)
        {
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    if (allVisitedTiles.Contains(allTiles[i][j]))
                    {
                        if (i != 0 && !allTiles[i - 1][j].blocked && !allVisitedTiles.Contains(allTiles[i - 1][j]))
                            allVisitedTiles.Add(allTiles[i - 1][j]);
                        if (i != 7 && !allTiles[i + 1][j].blocked && !allVisitedTiles.Contains(allTiles[i + 1][j]))
                            allVisitedTiles.Add(allTiles[i + 1][j]);
                        if (j != 0 && !allTiles[i][j - 1].blocked && !allVisitedTiles.Contains(allTiles[i][j - 1]))
                            allVisitedTiles.Add(allTiles[i][j - 1]);
                        if (j != 7 && !allTiles[i][j + 1].blocked && !allVisitedTiles.Contains(allTiles[i][j + 1]))
                            allVisitedTiles.Add(allTiles[i][j + 1]);
                    }
                }
            }
        }

        int goalx = (int)targetPos.x;
        int goaly = (int)targetPos.y;
        Vector2 first = new Vector2(goalx - 1, goaly);
        Vector2 second = new Vector2(goalx + 1, goaly);
        Vector2 third = new Vector2(goalx , goaly - 1);
        Vector2 fourth = new Vector2(goalx , goaly + 1);
        int minDistance = 100;
        
        foreach (var ele in allVisitedTiles)
        {
            if (differentMode)
            {
                if ((ele.position - targetPos).magnitude < minDistance)
                {
                    minDistance = (int)(ele.position - targetPos).magnitude;
                    result = ele.position;
                }
                
            }

            else
            {
                if (ele.position == first) return first;
                if (ele.position == second) return second;
                if (ele.position == third) return third;
                if (ele.position == fourth) return fourth;
            }
            
        }

        return result;
    }

    public void EndTurn()
    {
        dontRemoveTileSign.Clear();
        RemoveAllSignTiles();
        
    }

    public void RemoveAllTiles()
    {
        dontRemoveTileSign.Clear();
        RemoveAllSignTiles();
    }

    public void RemoveOwnTile(int x, int y)
    {
        if(!dontRemoveTileSign.Contains(new Vector2(x,y)))
        {
            RemoveTileAt(x,y,2);
        }
    }

    public void BeforeSwitchingMechs()
    {
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                allTiles[i][j].isInMoveRange = false;
            }
        }
    }

    public bool AreNeighbors(Vector2 first, Vector2 second)
    {
        return (Mathf.Abs(first.x - second.x) == 1 ^
            Mathf.Abs(first.y - second.y) == 1);
    }

    public void AddExplosion(int x, int y, int tileId, int layer)
    {
        Vector3Int pos = new Vector3Int(TILEMAP_START_X + y, TILEMAP_START_y - x, layer);
        tilemap.SetTile(pos, prefabTiles[tileId]);
        StartCoroutine(RemoveEplosionAt(x, y, layer));
    }

    private IEnumerator RemoveEplosionAt(int x, int y, int layer)
    {
        yield return new WaitForSeconds(2f);
        Vector3Int pos = new Vector3Int(TILEMAP_START_X + y, TILEMAP_START_y - x, layer);
        tilemap.SetTile(pos, null);
    }
    
    public static bool TileIsInMap(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        
        if (x < 0) return false;
        if (y < 0) return false;
        if (x > HEIGHT - 1) return false;
        if (y > WIDTH - 1) return false;

        return true;
    }
    
    public void ShowAttackCoordinates(Vector2 pos)
    {
       RemoveAllTiles();

        int x = (int)pos.x;
        int y = (int)pos.y;
        
        if(TileIsInMap(pos + new Vector2(1, 0)))AddTileAt(x+1,y,13,2);
        if(TileIsInMap(pos + new Vector2(-1, 0)))AddTileAt(x-1,y,13,2);
        if(TileIsInMap(pos + new Vector2(0, 1)))AddTileAt(x,y+1,13,2);
        if(TileIsInMap(pos + new Vector2(0, -1)))AddTileAt(x,y-1,13,2);
    }
}