using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static bool inIosView =true;
    // Start is called before the first frame update
    [SerializeField] private GameObject CityStuff1;
    [SerializeField] private GameObject CityStuff2;
    [SerializeField] private GameObject IsometricStuff;
    [SerializeField] private GameObject PlacingButtons1;
    [SerializeField] private GameObject PlacingButtons2;
    [SerializeField] private GameObject IsoMetricCanvas;
    [SerializeField] private GameObject GeneralBuilding;
            [Header("Refs")] 
    [SerializeField] private UnitManager _unitManager;
            [SerializeField] private GameObject MechResources1;
            [SerializeField] private GameObject MechResources2;

            [SerializeField] private GameObject allMechStuff;
            
            [SerializeField] private GameObject resource1; 
            [SerializeField] private GameObject resource2; 
    [Header("ISOSTUFF")] 
    [SerializeField] private GameObject buttonForSwitching;

    [Header("Mech UI")] 
    [SerializeField] private GameObject MechStuff1;
    [SerializeField] private GameObject MechStuff2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ButtonChecking();
    }

    public void SwitchBetweenView()
    {
        if (inIosView)
        {
            inIosView = false;
            int mechId = _unitManager.currentSelectedUnit.mechId;
            GeneralBuilding.SetActive(true);
            if(mechId==1)
            {
            
                CityStuff1.SetActive(true);
                PlacingButtons1.SetActive(true);
                resource1.SetActive(true);
            }
            else
            {
                CityStuff2.SetActive(true);
                PlacingButtons2.SetActive(true);
                resource2.SetActive(true);


            }
            IsometricStuff.SetActive(false);
            IsoMetricCanvas.SetActive(false);
            allMechStuff.SetActive(false);
            MechResources1.SetActive(false);
            MechResources2.SetActive(false);

        }
        else
        {
            resource1.SetActive(false);

            resource2.SetActive(false);

            GeneralBuilding.SetActive(false);
            inIosView = true;
            IsoMetricCanvas.SetActive(true);
            IsometricStuff.SetActive(true);
            CityStuff2.SetActive(false);
            CityStuff1.SetActive(false);
            PlacingButtons2.SetActive(false);
            PlacingButtons1.SetActive(false);

        }

    }
    
    private void ButtonChecking()
    {
        if (_unitManager.currentSelectedUnit != null) buttonForSwitching.SetActive(true);
        else buttonForSwitching.SetActive(false);
    }

    public void SwitchToMechView(int id)
    {
        if(id == 1)
        {
            MechResources1.SetActive(true);
            MechStuff1.SetActive(true);
        }
        else
        {
            MechResources1.SetActive(true);
            MechStuff2.SetActive(true);
        }
        allMechStuff.SetActive(true);

    }
    
    public void LeaveMechView()
    {
        MechStuff1.SetActive(false);
        MechStuff2.SetActive(false);
        allMechStuff.SetActive(false);
        MechResources1.SetActive(false);
        MechResources2.SetActive(false);

    }
}
