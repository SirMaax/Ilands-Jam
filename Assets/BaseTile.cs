using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour
{
    [Header("Attributes")] 
    public Vector2 position;
    public bool blocked = false;
    public int typeOfTile;
    public bool isInMoveRange;
    public bool EnemySpawning;
    
    [Header("Refs")] 
    private UnitManager _unitManager;
    // Start is called before the first frame update
    void Start()
    {
        _unitManager = GameObject.FindWithTag("UnitManager").GetComponent<UnitManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Vector2 position, int typeOfTile)
    {
        this.position = position;
        this.typeOfTile = typeOfTile;
    }
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && !blocked && isInMoveRange)
        {
            //Clicked on Unit
            
            _unitManager.ClickedOnThisTile(position);
        }
    }
}
