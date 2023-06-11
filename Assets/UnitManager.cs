using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("Attributes")] 
    public Unit currentSelectedUnit;

    [Header("refs")] 
    [SerializeField] private IsoMapManager _isoMapManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private ViewManager _viewManager;
    
    [Header("Arrays")] 
    [SerializeField] public Unit[] mechs;
    [SerializeField] private Unit[] buildings;
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
        StartCoroutine(CoolDownBeforeCanInteract(unit));
        
    }

    IEnumerator CoolDownBeforeCanInteract(Unit unit)
    {
        yield return new WaitForSeconds(0.1f);
        
        if (currentSelectedUnit != null)
        {
            CancelClickingOnUnit();
        }
        _isoMapManager.BeforeSwitchingMechs();
        _isoMapManager.RemoveAllSignTiles();
        currentSelectedUnit = unit;
        if (!unit.hasMovedThisTurn) _isoMapManager.ShowIfCanMoveTo(unit.position, unit.range);
        unit.SetStatusOfCollider(false);
        _viewManager.SwitchToMechView(unit.mechId);
    }
    
    public void CancelClickingOnUnit()
    {
        currentSelectedUnit.SetStatusOfCollider(true);
        currentSelectedUnit = null;
        _isoMapManager.RemoveAllSignTiles();
        _viewManager.LeaveMechView();
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

    public List<Unit> GetNextTarget(Vector2 pos, int range, int typeOfEnemy)
    {
        List<Unit> returnList = new List<Unit>();
        //TypeOfEnemy 1 is melee
        if (typeOfEnemy == 1)
        {
            foreach (var mech in mechs)
            {
                if (GetDistance(mech.position, pos) <= range + 1)returnList.Add(mech);
            }
            foreach (var mech in buildings)
            {
                if (GetDistance(mech.position, pos) <= range - 1)returnList.Add(mech);
            }
            //Iterate over buildings
        }
        //Calculate the shortest way for shooting something
        return returnList;
    }

    public Unit GetClosestTarget(Vector2 pos, int typeOfEnemy)
    {
        Unit returnUnit = null;
        if (typeOfEnemy == 1)
        {
            int shortestDistance = 100;
            
            if (typeOfEnemy == 1)
            {
                foreach (var mech in mechs)
                {
                    if (GetDistance(mech.position, pos) <= shortestDistance)
                    {
                        shortestDistance = GetDistance(mech.position, pos);
                        returnUnit = mech;
                    }
                }
                foreach (var mech in buildings)
                {
                    if (GetDistance(mech.position, pos) <= shortestDistance)
                    {
                        shortestDistance = GetDistance(mech.position, pos);
                        returnUnit = mech;
                    }
                }
                //Iterate over buildings
            }
        }
        return returnUnit;
    }

    int GetDistance(Vector2 pos, Vector2 otherPos)
    {
        return (int)(Mathf.Abs(pos.x - otherPos.x) + Mathf.Abs(pos.y - otherPos.y));
    }

    public Unit GetUnitAt(Vector2 pos)
    {
        foreach (var mech in mechs)
        {
            if (mech.position == pos) return mech;
        }
        foreach (var mech in buildings)
        {
            if (mech.position == pos) return mech;
        }
        foreach (var mech in _enemyManager.allEnemies)
        {
            if (mech.position == pos) return mech;
        }
        return null;
    }

    public void EndTurn()
    {
        foreach (var mech in mechs)
        {
            mech.EndTurn();
        }
    }
}
