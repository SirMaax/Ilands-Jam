using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
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
    [SerializeField] private TMP_Text descriptionField;
    [Header("status")] 
    private bool canClickOnThis = true;
    public bool hasMovedThisTurn = false;
    public Unit nextTarget;
    public Vector2 nextPostion;
    private Vector2 attackingField;
    private bool canAttack = false;
    public int attacking = 0;
    
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

    [Header("HP Looks")] 
    [SerializeField] private Sprite[] hp;
    [SerializeField] private GameObject mech1Overview;
    [SerializeField] private GameObject mech2Overview;
    

    // Start is called before the first frame update
    public void OwnStart()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        UpdatePosition(position);
        UpdateHealth();
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
            SetStatusOfCollider(true);
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
            attacking = 0;
            hasMovedThisTurn = false;
            SetStatusOfCollider(true);
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        UpdateHealth();
        if (health <= 0) Die();
        

    }

    private void Die()
    {
        if (typeOfEnemy != 0)
        {
            _unitManager.Remove(this);
            _isoMapManager.TileXisNowLongerBlocked(position);
            Destroy(this);
        }
        else
        {
            SetStatusOfCollider(false);
            this.enabled = false;
        }
        
    }

    public void Attack( Vector2 direction)
    {
        //Consume stuff

        if (!_resourceManager.EnoughResources(res.Steel, 1)
            || !_resourceManager.EnoughResources(res.Energy, 1)) return;
        _resourceManager.DecreaseIfEnough(res.Steel, 1);
        _resourceManager.DecreaseIfEnough(res.Energy, 1);
        
        //
        Vector2 location = new Vector2(direction.x + position.x, direction.y + position.y);
        _unitManager.DamageBeingAt(location, 2);
        
        //Dmg Animation at location
        _isoMapManager.AddExplosion((int)location.x, (int)location.y, 12, 4);
        attacking = 0;
        descriptionField.SetText("");
    }
    
    public void AttackRanged( Vector2 direction)
    {
        //Consume stuff
        if (!_resourceManager.EnoughResources(res.Ammo, 1)
            || !_resourceManager.EnoughResources(res.Energy, 1)) return;
        _resourceManager.DecreaseIfEnough(res.Ammo, 1);
        _resourceManager.DecreaseIfEnough(res.Energy, 1);
        
        //
        Vector2 location = new Vector2(direction.x + position.x, direction.y + position.y);
        while (!_unitManager.DamageBeingAt(location, 1))
        {
            location += direction;
            if (!IsoMapManager.TileIsInMap(location))
            {
                location -= direction;
                break;
            }
        }
        
        //Dmg Animation at location
        _isoMapManager.AddExplosion((int)location.x, (int)location.y, 12, 4);
        attacking = 0;
        descriptionField.SetText("");
    }

    public void Attack()
    {
        attacking = 1;
        _unitManager.AttackModeTurnedOn();
        descriptionField.SetText("Attack a Tile in front of the unit. Deals 2 dmg.\nCost one Steel and one Energy");

    }

    public void AttackRanged()
    {
        attacking = 2;
        _unitManager.AttackModeTurnedOn();
        descriptionField.SetText("Shoots a projectile in a direction. Deals 1 dmg.\nCost one Ammo and one Energy.");

    }

    public void UpdateHealth()
    {
        Sprite currentSprite = hp[health];
        if (mechId == 1)  mech1Overview.GetComponent<SpriteRenderer>().sprite = currentSprite; 
        else if (mechId == 2)  mech2Overview.GetComponent<SpriteRenderer>().sprite = currentSprite; 
    }
}