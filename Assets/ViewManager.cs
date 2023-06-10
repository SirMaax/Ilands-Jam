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
    [SerializeField] private GameObject ResourceBar1;
    [SerializeField] private GameObject ResourceBar2;

    
        [Header("Refs")] 
    [SerializeField] private UnitManager _unitManager;
    
    [Header("ISOSTUFF")] 
    [SerializeField] private GameObject buttonForSwitching;
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
            if(mechId==1)
            {
            
                CityStuff1.SetActive(true);
                PlacingButtons1.SetActive(true);
                ResourceBar1.SetActive(true); 
            }
            else
            {
                CityStuff2.SetActive(true);
                PlacingButtons2.SetActive(true);
                ResourceBar2.SetActive(true); 


            }
            IsometricStuff.SetActive(false);
        }
        else
        {
            inIosView = true;
            IsometricStuff.SetActive(true);
            CityStuff2.SetActive(false);
            CityStuff1.SetActive(false);
            PlacingButtons2.SetActive(false);
            PlacingButtons1.SetActive(false);
            ResourceBar1.SetActive(false); 


        }

    }
    
    private void ButtonChecking()
    {
        if (_unitManager.currentSelectedUnit != null) buttonForSwitching.SetActive(true);
        else buttonForSwitching.SetActive(false);
    }
}
