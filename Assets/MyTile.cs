using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MyTile: MonoBehaviour
{
    public Vector2 position;
    public int typeOfCell;
    private TileVisuals tileVisuals;
    private SpriteRenderer sp;
    private MouseManager _mouseManager;

    public bool isBuilding = false;
    public bool isBlocked = false;
    public bool isResource = false;
    private void OnMouseOver()
    {
        
        _mouseManager.mousePosGrid = position;
    }

    public void Init()
    {
        _mouseManager = GameObject.FindWithTag("MouseManager").GetComponent<MouseManager>();
        tileVisuals = GameObject.FindWithTag("TileVisuals").GetComponent<TileVisuals>();
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = tileVisuals.GetSprite(typeOfCell);
        gameObject.transform.localScale = new Vector3(TileManager.GLOBAL_CELLSIZE, TileManager.GLOBAL_CELLSIZE, 1);
    }


}
