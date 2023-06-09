using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MyTile: MonoBehaviour
{
    [Header("Refs")]
    private MouseManager _mouseManager;

    [Header("Own Attributes")]
    public Vector2 position;
    public int typeOfCell;
    private TileVisuals tileVisuals;
    private SpriteRenderer sp;

    public bool isBuilding = false;
    public bool isBlocked = false;
    public bool isResource = false;
    private void OnMouseEnter()
    {
        if (typeOfCell == 3 || typeOfCell == 2) return;
        _mouseManager.mousePosGrid = position;
    }


    public void Start()
    {
        _mouseManager = GameObject.FindWithTag("MouseManager").GetComponent<MouseManager>();
        tileVisuals = GameObject.FindWithTag("TileVisuals").GetComponent<TileVisuals>();
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = tileVisuals.GetSprite(typeOfCell);
    }

    public void Init()
    {
        gameObject.transform.localScale = new Vector3(TileManager.GLOBAL_CELLSIZE, TileManager.GLOBAL_CELLSIZE, 1);
        
    }

    public void UpdateSprite()
    {
        
        sp.sprite = tileVisuals.GetSprite(typeOfCell);
    }

    public void UpdateTile(int newtype)
    {
        typeOfCell = newtype;
        UpdateSprite();
    }
}
