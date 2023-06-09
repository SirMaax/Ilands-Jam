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
    
    
    public void Init()
    {
        tileVisuals = GameObject.FindWithTag("TileVisuals").GetComponent<TileVisuals>();
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = tileVisuals.GetSprite(typeOfCell);
        gameObject.transform.localScale = new Vector3(TileManager.GLOBAL_CELLSIZE, TileManager.GLOBAL_CELLSIZE, 1);
    }


}
