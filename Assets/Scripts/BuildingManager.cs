using System.Collections;
using TMPro;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private bool test = false;
    
    [Header("Refs")]
    [SerializeField]private MouseManager _mouseManager;
    [SerializeField]private TileManager _tileManager;
    [SerializeField]private ResourceManager _resourceManager;
    [SerializeField] private TMP_Text description;
    [SerializeField] private string[] descriptions;
    
    [Header("PlacingStuff")]
    [SerializeField] private GameObject tilePrefab;
    private GameObject currentTempGameObjekt;
    private bool placingBuilding = false;
    private int currentBuildingId;
    private bool cooldown = false;
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
        if (placingBuilding && Input.GetMouseButton(0))
        {
            if(_tileManager.TileAvailable(currentBuildingId, _mouseManager.mousePosGrid)
               && _mouseManager.MouseIsInField()
               && cooldown)            PlaceBuilding();
        }
    }

    public void StartPlaceBuilding(int buildingId)
    {
        if (placingBuilding)
        {
            CancelPlacBuilding();
        }
        cooldown = false;
        StartCoroutine(StartCooldown());
        description.SetText(descriptions[buildingId]);
        _mouseManager.mousePosGrid = new Vector2(-1,-1);
        placingBuilding = true;
        currentBuildingId = buildingId;
        currentTempGameObjekt = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
        // currentTempGameObjekt.transform.position =
        //     new Vector3(_mouseManager.mousePosGrid.x, _mouseManager.mousePosGrid.y, -1);
        MyTile tempFile = currentTempGameObjekt.GetComponent<MyTile>();
        currentTempGameObjekt.GetComponent<SpriteRenderer>().sortingOrder = 1;
        tempFile.GetComponent<BoxCollider2D>().enabled = false;
        tempFile.typeOfCell = 2;
        tempFile.Start();
        
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        cooldown = true;
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
        if (_mouseManager.mousePosGrid.x == -1) return;
        _tileManager.ChangeCell(_mouseManager.mousePosGrid, currentBuildingId);
        placingBuilding = false;
        currentBuildingId = -1;
        _tileManager.CheckEnergy();
        description.SetText("");
        Destroy(currentTempGameObjekt);
        if (currentBuildingId == 15 || currentBuildingId == 22)
        {
            
            _tileManager.CheckEnergy();
        }
        SoundManager.Play(6);
    }

    void CancelPlacBuilding()
    {
        description.SetText("");
        placingBuilding = false;
        currentBuildingId = -1;
        Destroy(currentTempGameObjekt);
    }
    
    
}