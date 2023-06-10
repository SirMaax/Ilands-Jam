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

    [Header("Map Stuff")] private BaseTile[][] allTiles;
    private int[][] typeofTiles;
    public static int WIDTH = 8;
    public static int HEIGHT = 8;
    [Header("Refs")] [SerializeField] private GameObject baseTilePrefab;
    [SerializeField] private Vector2 firstTile;
    [SerializeField] private int TILEMAP_START_X;
    [SerializeField] private int TILEMAP_START_y;


    [Header("Tiles")] [SerializeField] private TileBase[] prefabTiles;

    [SerializeField] public Tilemap tilemap;

    // Start is called before the first frame update
    public void OwnStart()
    {
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
                        if(!allMarkedTiles.Contains(allTiles[i][j])) allMarkedTiles.Add(allTiles[i][j]);
                    }
                }
            }

            foreach (var ele in allMarkedTiles)
            {
                if (ele.blocked && ele.position != isogridPos) continue;
                if(ele.position == isogridPos)MarkAllNeighborsOf2((int) ele.position.x, (int)ele.position.y);
                else MarkAllNeighborsOf((int) ele.position.x, (int)ele.position.y);
            }
        }
        foreach (var ele in allMarkedTiles)
        {
            if (ele.position == isogridPos) continue;
            if (ele.blocked) AddTileAt((int)ele.position.x, (int)ele.position.y, 1, 2);
            else AddTileAt((int)ele.position.x, (int)ele.position.y, 2, 2);
        }
        RemoveTileAt(x,y,2);
    }

    public void MarkAllNeighborsOf(int i, int j)
    {
        if (i != 0 && !allTiles[i - 1][j].blocked) allTiles[i - 1][j].isInMoveRange = true;
        else if (i != 0 && allTiles[i - 1][j].blocked) AddTileAt(i - 1, j, 1, 2);
        if (i != 7 && !allTiles[i + 1][j].blocked) allTiles[i + 1][j].isInMoveRange = true;
        else if (i != 7 && !allTiles[i + 1][j].blocked)AddTileAt(i + 1, j, 1, 2);
        if (j != 0 && !allTiles[i ][j - 1].blocked) allTiles[i][j - 1].isInMoveRange = true;
        else  if (j != 0 && !allTiles[i ][j - 1].blocked)AddTileAt(i, j -1, 1, 2);
        if (j != 7 && !allTiles[i ][j + 1].blocked) allTiles[i][j + 1].isInMoveRange = true;
        else if (j != 7 && !allTiles[i ][j + 1].blocked)AddTileAt(i, j +1, 1, 2);
    }
    
    public void MarkAllNeighborsOf2(int i, int j)
    {
        if (i != 0 && !allTiles[i - 1][j].blocked) allTiles[i - 1][j].isInMoveRange = true;
        if (i != 7 && !allTiles[i + 1][j].blocked) allTiles[i + 1][j].isInMoveRange = true;
        if (j != 0 && !allTiles[i ][j - 1].blocked) allTiles[i][j - 1].isInMoveRange = true;
        if (j != 7 && !allTiles[i ][j + 1].blocked) allTiles[i][j + 1].isInMoveRange = true;
    }
    
    public void RemoveAllSignTiles()
    {
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                RemoveTileAt(i, j, 2);
                allTiles[i][j].isInMoveRange = false;
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
}