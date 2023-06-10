using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private bool test = false;
    
    [Header("Refs")]
    [SerializeField]private MouseManager _mouseManager;
    [SerializeField]private TileManager _tileManager;
    [SerializeField]private ResourceManager _resourceManager;
    
    [Header("PlacingStuff")]
    [SerializeField] private GameObject tilePrefab;
    private GameObject currentTempGameObjekt;
    private bool placingBuilding = false;
    private int currentBuildingId;
    // Start is called before the first frame update
    void Start()
    {
        // _mouseManager = GameObject.FindWithTag("MouseManager").GetComponent<MouseManager>();
        // _tileManager = GameObject.FindWithTag("TileManager1").GetComponent<TileManager>();
        // _resourceManager = GameObject.FindWithTag("ResourceManager1").GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = false;
            StartPlaceBuilding(0);
        }
        
        if (Input.GetMouseButton(1) && placingBuilding) CancelPlacBuilding();
        if (placingBuilding)UpdateBuildingPositon();
        if (placingBuilding && Input.GetMouseButton(0)
                            && _tileManager.TileAvailable(currentBuildingId, _mouseManager.mousePosGrid)
                            && _mouseManager.MouseIsInField())
            PlaceBuilding();
    }

    public void StartPlaceBuilding(int buildingId)
    {
        
        _mouseManager.mousePosGrid = new Vector2(-1,-1);
        placingBuilding = true;
        currentBuildingId = buildingId;
        currentTempGameObjekt = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
        // currentTempGameObjekt.transform.position =
        //     new Vector3(_mouseManager.mousePosGrid.x, _mouseManager.mousePosGrid.y, -1);
        MyTile tempFile = currentTempGameObjekt.GetComponent<MyTile>();
        tempFile.typeOfCell = 2;
        tempFile.Init(_tileManager);
        tempFile.Start();
        
    }

    private void UpdateBuildingPositon()
    {
        if(_mouseManager.mousePosGrid.x != -1)
        {
            var newPos = _tileManager.GetRealWorldCoordinates(_mouseManager.mousePosGrid);
            currentTempGameObjekt.transform.position =
                new Vector3(newPos.x, newPos.y, -1);
            MyTile tempFile = currentTempGameObjekt.GetComponent<MyTile>();
            
            if (_tileManager.TileAvailable(currentBuildingId, _mouseManager.mousePosGrid))
            {
                //Green Shine
                tempFile.typeOfCell = 2;
                tempFile.UpdateSprite();
                
            }
            else
            {
                tempFile.typeOfCell = 3;
                tempFile.UpdateSprite();
            }
        }
        else
        {
            currentTempGameObjekt.transform.position = (Vector3)_mouseManager.mousePosition + Vector3.back;
        }
        
    }

    void PlaceBuilding()
    {
        if (!_resourceManager.HasEnoughResourcesFor(currentBuildingId)) return;
        _tileManager.ChangeCell(_mouseManager.mousePosGrid, currentBuildingId);
        placingBuilding = false;
        currentBuildingId = -1;
        _tileManager.CheckEnergy();
        Destroy(currentTempGameObjekt);
    }

    void CancelPlacBuilding()
    {
        placingBuilding = false;
        currentBuildingId = -1;
        Destroy(currentTempGameObjekt);
    }
    
    
}