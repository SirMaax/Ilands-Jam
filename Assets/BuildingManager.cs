using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private MouseManager _mouseManager;
    private TileManager _tileManager;
    [SerializeField] private GameObject tilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        _mouseManager = GameObject.FindWithTag("MouseManager").GetComponent<MouseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaceBuilding(int buildingId)
    {
        GameObject tempTile = Instantiate(tilePrefab,Vector3.zero, Quaternion.identity);
        tempTile.transform.position = new Vector3(0,0,1) +(Vector3) _mouseManager.mousePosGrid;

        if (_tileManager.TileAvailable(buildingId, _mouseManager.mousePosGrid))
        {
            //Green Shine
        }
        else
        {
            //RED SHINE
        }
    }
}
