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

    [Header("Refs")] [SerializeField] private TileManager TileManager1;
    [SerializeField] private TileManager TileManager2;
        [SerializeField] private ResourceManager ResourceManager1;
        [SerializeField] private ResourceManager ResourceManager2;

    // Start is called before the first frame update
    void Start()
    {
        StartOnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndTurn()
    {
        TileManager1.EndTurn();
        TileManager2.EndTurn();
        ResourceManager1.EndTurn();
        ResourceManager2.EndTurn();
    }

    private void StartOnObjects()
    {
        start1.OwnStart();
        start2.OwnStart();
        start3.OwnStart();
    }
    
    
}

