using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool TESTING = true;

    [Header("ExecutionOrder")] 
    [SerializeField] private IsoMapManager start1;
    [SerializeField] private Unit start2;
    [SerializeField] private Unit start3;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Unit island1;
    [SerializeField] private Unit island2;
    
    [Header("Refs")]
    [SerializeField] private TileManager TileManager1;
    [SerializeField] private TileManager TileManager2;
    [SerializeField] private ResourceManager ResourceManager2;
    [SerializeField] private ResourceManager ResourceManager1;
    [SerializeField] private IsoMapManager IsoMapManager;
    [SerializeField] private UnitManager _unitManager;

    public static int currentTurn;
    // Start is called before the first frame update
    void Start()
    {
        StartOnObjects();
        // BeginningTurn();
        currentTurn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_unitManager.buildings.Count == 0 || _unitManager.mechs.Count == 0)
        {
            //Gameover
            Debug.Log("gameover");
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
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
        currentTurn++;
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
        _enemyManager.OwnStart(2);
        // _enemyManager.Test();
        island1.OwnStart();
        island2.OwnStart();
    }
    
    
}

