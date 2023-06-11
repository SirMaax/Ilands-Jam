using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    [Header("Unit Attribute")] [SerializeField]
    public Vector2 position;

    [SerializeField] private int typeOfUnit;
    [SerializeField] private int layerId;
    [SerializeField] public int unitId;
    [SerializeField] private bool canCollide;

    [Header("status")] 
    private bool canClickOnThis = true;
    public bool hasMovedThisTurn = false;
    public Unit nextTarget;
    public Vector2 nextPostion;
    private Vector2 attackingField;
    private bool canAttack = false;
    
    [Header("General Attributes")] [SerializeField]
    private int health;

    [SerializeField] private int dmg;


    [Header("Enemy Attributes")] [SerializeField]
    public int typeOfEnemy;


    [Header("Mech Attributes")]
    [SerializeField] public int range;
    [SerializeField] public int mechId;


    [Header("REfs")] 
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] public IsoMapManager _isoMapManager;
    [SerializeField] public UnitManager _unitManager;
    private BoxCollider2D _boxCollider2D;


    // Start is called before the first frame update
    public void OwnStart()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        UpdatePosition(position);
    }

    public void Init()
    {
        UpdatePosition(position);
        if(!canCollide)SetStatusOfCollider(false);
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void UpdatePosition(Vector2 gridPos)
    {
        _isoMapManager.RemoveOwnTile((int)position.x, (int)position.y);
        _isoMapManager.allTiles[(int)position.x][(int)position.y].blocked = false;
        // _isoMapManager.RemoveTileAt((int)position.x, (int)position.y, 2);
        _isoMapManager.RemoveTileAt((int)position.x, (int)position.y, layerId);
        position = gridPos;
        Vector2 newPos = _isoMapManager.GetRealWorldCords(gridPos) + new Vector2(0, 0.25f);
        transform.position = newPos;
        _isoMapManager.AddTileAt((int)position.x, (int)position.y, unitId, layerId);
        _isoMapManager.TileXisNowBlocked(position);
    }

    public void ToggleCollider()
    {
        if (_boxCollider2D.enabled) _boxCollider2D.enabled = false;
        else _boxCollider2D.enabled = true;
    }

    public void SetStatusOfCollider(bool status)
    {
        if (!canCollide) return;
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

    public void PreEnemyTurn()
    {
        canAttack = false;
        ChooseTarget();
        MoveToTarget();
    }

    private void ChooseTarget()
    {
        //Find closest Target 
        List<Unit> targets = new List<Unit>();
        targets = _unitManager.GetNextTarget(position, range, typeOfEnemy);

        if (targets.Count >= 1)
        {
            foreach (var ele in targets)
            {
                Vector2 pos = _isoMapManager.CanReach(position, ele.position, range);
                if (pos != new Vector2(-1, -1))
                {
                    nextPostion = pos;
                    nextTarget = ele;
                    canAttack = true;
                    break;
                }
            }
            // nextTarget = targets[(int)Random.Range(0f, targets.Count)];
        }
        else
        {
            nextTarget = _unitManager.GetClosestTarget(position, typeOfEnemy);
            nextPostion = _isoMapManager.CanReach(position, nextTarget.position, range, true);
        }
    }

    private void MoveToTarget()
    {
        UpdatePosition(nextPostion);
        //Mark field for attack
        if(canAttack) _isoMapManager.AddTileAt((int)nextTarget.position.x, (int)nextTarget.position.y, 9, 2);
    }

    public void EndTurn()
    {
        if (typeOfEnemy != 0)
        {
            Unit temp = _unitManager.GetUnitAt(attackingField);
            if (temp != null)
            {
                temp.Damage(dmg);
            }
        }
        else
        {//MECH CASE
            hasMovedThisTurn = false;
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        if (health <= 0) Die();
    }

    private void Die()
    {
    }
}