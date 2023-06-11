using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MyTile: MonoBehaviour
{
    [Header("Refs")]
    private MouseManager _mouseManager;
    private ResourceManager _resourceManager;
    private TileManager _tileManager;
    
    [Header("Own Attributes")]
    public Vector2 position;
    public int typeOfCell;
    private TileVisuals tileVisuals;
    private SpriteRenderer sp;

    [Header("Status")]
    public bool isBuilding = false;
    public bool isBlocked = false;
    public bool isResource = false;
    public bool isPowered;

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
        if (typeOfCell == 16 || typeOfCell == 17) isBlocked = true;
        else if (typeOfCell > 3 && typeOfCell < 19) isBuilding = true;
        else if (typeOfCell > 18) isResource = true;
        isPowered = false;
    }

    public void Init(TileManager tileManager, ResourceManager resourceManager)
    {
        _tileManager = tileManager;
        _resourceManager = resourceManager;

        gameObject.transform.localScale = new Vector3(TileManager.GLOBAL_CELLSIZE, TileManager.GLOBAL_CELLSIZE, 1);
        
    }

    private void Update()
    {
        
    }

    public void UpdateSprite()
    {
        if (typeOfCell == 11) return;
        Debug.Log(typeOfCell);
        if (typeOfCell == 15 && !isPowered) sp.sprite = tileVisuals.GetSprite(22);
        else sp.sprite = tileVisuals.GetSprite(typeOfCell);
    }

    public void UpdateTile(int newtype)
    {
        typeOfCell = newtype;
        isBuilding = false;
        isResource = false;
        isPowered = false;
        if (typeOfCell > 3) isBuilding = true;
        else if (typeOfCell > 18) isResource = true;
        UpdateSprite();
        
    }

    public void Turn()
    {
        if (!IsInPylonRange()) return;
        switch (typeOfCell)
        {
            case 4://Copper
                _resourceManager.Increase(4);
                break;
            case 5://Iron
                _resourceManager.Increase(5);
                break;
            case 6://Uran
                _resourceManager.Increase(6);
                break;
            case 7://Steel Smelting
                _resourceManager.Increase(7);
                break;
            case 8://Sheel Production
                _resourceManager.Increase(8);
                break;
            case 9://Nuke Prod
                _resourceManager.Increase(9);
                break;
            case 10://Research
                _resourceManager.Research();
                break;
            case 18://Research
                _resourceManager.Increase(18,2);
                break;
        }
    }

    public bool IsInPylonRange()
    {
        return _tileManager.IsPoweredPylonInRange(position);
    }

    public bool IsPoweredPylon()
    {
        return (typeOfCell == 15 && isPowered) || typeOfCell == 11 || typeOfCell == 12;
    }


}
