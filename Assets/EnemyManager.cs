using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("List")] public List<Unit> allEnemies;
    public List<Unit> sleepingUnits;

    [Header("Refs")] [SerializeField] private IsoMapManager _isoMapManager;
    [SerializeField] private UnitManager _unitManager;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Attributes")] [SerializeField]
    private int spawnEmeiesEveryTurn;

    // Start is called before the first frame update
    public void OwnStart(int amountEnemies)
    {
        sleepingUnits = new List<Unit>();
        for (int i = 0; i < amountEnemies; i++)
        {
            SpawnEnemy(1, 10);
        }
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void EndTurn()
    {
        foreach (var enemy in allEnemies)
        {
            enemy.EndTurn();
        }
    }

    public void SpawnEnemy(int typeOfEnemy, int IosId)
    {
        int tries = 0;
        while (tries < 100)
        {
            int x = Random.Range(0, 8);
            int y = Random.Range(0, 8);
            Vector2 pos = new Vector2(x, y);
            if (!_isoMapManager.allTiles[x][y].blocked)
            {
                _isoMapManager.allTiles[x][y].blocked = true;
                _isoMapManager.allTiles[x][y].EnemySpawning = true;
                _isoMapManager.AddTileAt(x, y, 11, 2);

                GameObject newGameObj = Instantiate(enemyPrefab, pos, Quaternion.identity);
                newGameObj.transform.rotation = Quaternion.Euler(0, 0, 45);
                Unit enemy = newGameObj.GetComponent<Unit>();

                enemy.position = pos;
                enemy.typeOfEnemy = typeOfEnemy;
                enemy.unitId = IosId; //Base Enemy
                enemy._isoMapManager = _isoMapManager;
                enemy._unitManager = _unitManager;
                enemy.enabled = false;
                // enemy.Init();
                sleepingUnits.Add(enemy);
                break;
            }

            tries++;
        }
    }

    public void SpawnEnemyAt(int typeOfEnemy, int IosId, int x, int y)
    {
        int tries = 0;
        while (tries < 100)
        {
            Vector2 pos = new Vector2(x, y);
            if (!_isoMapManager.allTiles[x][y].blocked)
            {
                GameObject newGameObj = Instantiate(enemyPrefab, pos, Quaternion.identity);
                newGameObj.transform.rotation = Quaternion.Euler(0, 0, 45);
                Unit enemy = newGameObj.GetComponent<Unit>();
                allEnemies.Add(enemy);
                enemy.position = pos;
                // enemy.GetComponent<BoxCollider2D>().enabled = false;
                enemy.typeOfEnemy = typeOfEnemy;
                enemy.unitId = IosId; //Base Enemy
                enemy._isoMapManager = _isoMapManager;
                enemy._unitManager = _unitManager;
                enemy.Init();
                break;
            }

            tries++;
        }
    }

    public void BeginningTurn()
    {
        if (GameManager.currentTurn >= 2) spawnEmeiesEveryTurn = 2;
        if (GameManager.currentTurn >= 4) spawnEmeiesEveryTurn = 3;
        if (GameManager.currentTurn >= 6) spawnEmeiesEveryTurn = 4;
        if (GameManager.currentTurn >= 8) spawnEmeiesEveryTurn = 5;
        if (GameManager.currentTurn >= 10) spawnEmeiesEveryTurn = 6;



        foreach (var ele in sleepingUnits)
        {
            ele.Init();
            allEnemies.Add(ele);
            _isoMapManager.allTiles[(int)ele.position.x][(int)ele.position.y].EnemySpawning = false;
        }

        sleepingUnits.Clear();
        for (int i = 0; i < spawnEmeiesEveryTurn; i++)
        {
            SpawnEnemy(1, 10);
        }

        foreach (var enemy in allEnemies)
        {
            enemy.PreEnemyTurn();
        }
    }

    public void KillAll()
    {
        bool killedSomething = true;
        while (killedSomething)
            killedSomething = false;
        foreach (var enemy in allEnemies)
        {
            // enemy.health = 0;
            enemy.Damage(99);
            killedSomething = true;
            break;
        }
    }
}