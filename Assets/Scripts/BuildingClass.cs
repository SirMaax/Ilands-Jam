using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingClass : MonoBehaviour
{
    [SerializeField] private int[] resourceBuildings;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool ResourceBuilding(int buildingId)
    {
        return resourceBuildings.Contains(buildingId);
    }

    public bool CanMineResource(int buildingId, int otherTile)
    {
        //Do something
        return false;
    }


}
