using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("Attributes")] 
    public Unit currentSelectedUnit;

    [Header("refs")] 
    [SerializeField] private IsoMapManager _isoMapManager; 
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSelectedUnit != null && Input.GetMouseButton(1)
            && ViewManager.inIosView) CancelClickingOnUnit();
    }

    public void ClickedOnThisUnit(Unit unit)
    {
        
        if (unit.hasMovedThisTurn) return;
        if (currentSelectedUnit != null)
        {
            CancelClickingOnUnit();
        }
        currentSelectedUnit = unit;
        if(!unit.hasMovedThisTurn)_isoMapManager.ShowIfCanMoveTo(unit.position,unit.range);
        // DisableCollidersOfOthers();
        unit.SetStatusOfCollider(false);
    }

    public void CancelClickingOnUnit()
    {
        currentSelectedUnit.SetStatusOfCollider(true);
        currentSelectedUnit = null;
        _isoMapManager.RemoveAllSignTiles();
        
        // EnableAllCollidersOfOthers();
    }

    public void ClickedOnThisTile(Vector2 pos)
    {
        if (currentSelectedUnit != null)
        {
            currentSelectedUnit.MoveToPos(pos);
            CancelClickingOnUnit();
        }
    }

    private void DisableCollidersOfOthers()
    {
        foreach (var ele in GameObject.FindGameObjectsWithTag("Unit"))
        {
            ele.GetComponent<Unit>().SetStatusOfCollider(false);
        }
    }
    private void EnableAllCollidersOfOthers()
    {
        foreach (var ele in GameObject.FindGameObjectsWithTag("Unit"))
        {
            ele.GetComponent<Unit>().SetStatusOfCollider(true);
        }
    }
    
    
}
