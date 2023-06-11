using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] public Vector2 mousePosition;
    [SerializeField] public Vector2 mousePosGrid;
    [SerializeField] public Vector2 gridStart;
    [SerializeField] public float startX;
    [SerializeField] public float startY;
    [SerializeField] public float endX;
    [SerializeField] public float endY;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mousePosGrid = new Vector2(-1, -1);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    
    public bool MouseIsInField()
    {
        if (mousePosition.x > startX && mousePosition.x < endX
                                     && mousePosition.y < startY && mousePosition.y > endY) return true;
        // if (mousePosition.x > TileManager.GLOBAL_GRID_START.x - (TileManager.GLOBAL_CELLSIZE)
        //     && mousePosition.x < TileManager.GLOBAL_GRID_START.x + TileManager.GLOBAL_CELLSIZE / 2 +
        //     (TileManager.GLOBAL_WIDTH-1) * TileManager.GLOBAL_CELLSIZE
        //     && mousePosition.y < TileManager.GLOBAL_GRID_START.y + TileManager.GLOBAL_CELLSIZE / 2
        //     && mousePosition.y > TileManager.GLOBAL_GRID_START.y - TileManager.GLOBAL_CELLSIZE / 2 -
        //     (TileManager.GLOBAL_HEIGHT-1 )* TileManager.GLOBAL_CELLSIZE
        //    ) return true;
        return false;
    }
}
