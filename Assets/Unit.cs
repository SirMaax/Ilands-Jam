using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    [Header("Unit Attribute")] 
    [SerializeField]public Vector2 position;
    [SerializeField] private int typeOfUnit;
    [SerializeField] private int layerId;
    [SerializeField] int unitId;
    [SerializeField] private bool canCollide;

    [Header("status")] 
    private bool canClickOnThis = true;
    public bool hasMovedThisTurn = false;
    
    [Header("Mech Attributes")]
    [SerializeField] public int range;
    [SerializeField] public int mechId;

    [Header("REfs")] 
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private IsoMapManager _isoMapManager;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] UnitManager _unitManager;
    // Start is called before the first frame update
    public void OwnStart()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        UpdatePosition(position);
    }

    public void Init()
    {
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePosition(Vector2 gridPos)
    {
        _isoMapManager.RemoveTileAt((int)position.x, (int)position.y,layerId);
        position = gridPos;
        Vector2 newPos =_isoMapManager.GetRealWorldCords(gridPos) + new Vector2(0, 0.25f);
        transform.position = newPos;
        _isoMapManager.AddTileAt((int)position.x, (int)position.y, unitId,layerId);
        _isoMapManager.TileXisNowBlocked(position);
    }

    public void ToggleCollider()
    {
        if (_boxCollider2D.enabled) _boxCollider2D.enabled = false;
        else _boxCollider2D.enabled = true;
    }

    public void SetStatusOfCollider(bool status)
    {
        _boxCollider2D.enabled = status;
    }

    private void OnMouseOver()
    {
        if (!canCollide) return;
        if (Input.GetMouseButton(0) && canClickOnThis)
        {
            canClickOnThis = false;
            StartCoroutine(ClickCooldown());
            _unitManager.ClickedOnThisUnit(this);
            //Pressed on unit
        }
    }

    IEnumerator ClickCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        canClickOnThis = true;
    }

    public void MoveToPos(Vector2 newPos)
    {
        if (_resourceManager.DecreaseIfEnough(res.Energy, 1))
        {
            _isoMapManager.TileXisNowLongerBlocked(position);
            UpdatePosition(newPos);
            hasMovedThisTurn = true;
        }
    }
}
