using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool TESTING = true;

    [Header("ExecutionOrder")] 
    [SerializeField] private IsoMapManager start1;
    [SerializeField] private Unit start2;
    [SerializeField] private Unit start3;
    [SerializeField] private EnemyManager _enemyManager;
    
    [Header("Refs")]
    [SerializeField] private TileManager TileManager1;
    [SerializeField] private TileManager TileManager2;
    [SerializeField] private ResourceManager ResourceManager2;
    [SerializeField] private ResourceManager ResourceManager1;
    [SerializeField] private IsoMapManager IsoMapManager;
    [SerializeField] private UnitManager _unitManager;
    // Start is called before the first frame update
    void Start()
    {
        StartOnObjects();
        // BeginningTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndTurn()
    {
        TileManager1.EndTurn();
        TileManager2.EndTurn();
        //Enemies
        //BlockInputEtcTillEnemies down
        IsoMapManager.EndTurn();
        _enemyManager.EndTurn();
        _unitManager.EndTurn();
        BeginningTurn();
    }

    public void BeginningTurn()
    {
        _enemyManager.BeginningTurn();
    }

    private void StartOnObjects()
    {
        start1.OwnStart();
        start2.OwnStart();
        start3.OwnStart();
        _enemyManager.OwnStart(1);
        // _enemyManager.Test();
    }
    
    
}

