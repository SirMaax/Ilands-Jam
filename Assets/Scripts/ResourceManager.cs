using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{

    [SerializeField] private bool test;
    [SerializeField] private int[] resources;

    [SerializeField] private TMP_Text[] howMuchOfEachResource;
    // Start is called before the first frame update
    void Start()
    {
        if (test)
        {
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = 99;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasEnoughResourcesFor(int building)
    {
        if (GameManager.TESTING) return true;
        switch (building)
        {
            case res.CopperOre://Copper mine 1 Steel
                if (resources[res.Steel] - 1 >= 0)
                {
                    resources[res.Steel]--;
                    return true;
                }
                break;
            case res.IronOre://Iron Mine one steel one copper
                if (resources[res.Steel] - 1 >= 0 
                    && resources[res.CopperOre] -1 >= 0)
                {
                    resources[res.Steel]--;
                    resources[res.CopperOre]--;
                    return true;
                }
                break;
            case res.UraniumOre://Uranium Mine 3 Steel and 2 copper
                if (resources[res.IronOre] - 3 >= 0
                    && resources[res.CopperOre]- 2 >= 0)
                {
                    resources[res.IronOre]-=3;
                    resources[res.CopperOre] -= 2;
                    return true;
                }
                break;
            case res.Steel://Steel Production -1 iron ore 2 steel
                if (resources[res.IronOre] - 1 >= 0
                    && resources[res.Steel] - 2>= 0)
                {
                    resources[res.Steel] -= 2;
                    resources[res.IronOre]--;
                    return true;
                }
                break;
            case res.Ammo://Ammo Production
                if (resources[res.CopperOre] - 2 >= 0
                    && resources[res.Steel] - 2>= 0)
                {
                    resources[res.Steel] -= 2;
                    resources[res.CopperOre]-=2;
                    return true;
                }
                break;
            case res.Nuke://Nuke Prod
                if (resources[res.UraniumOre] - 1 >= 0
                    && resources[res.Steel] - 3 >= 0
                    && resources[res.CopperOre] - 2 >= 0)
                {
                    resources[res.Steel] -= 3;
                    resources[res.UraniumOre] -= 1;
                    resources[res.CopperOre] -= 3;
                    return true;
                }
                break;
            case res.Research://Research Building
                if (resources[res.CopperOre] - 5 >= 0
                    && resources[res.Steel] - 2 >= 0)
                {
                    resources[res.Steel] -= 2;
                    resources[res.CopperOre] -= 5;
                    return true;
                }
                break;
            case 15: //Pylon
                if (resources[res.CopperOre] - 2 >= 0)
                {
                    resources[res.CopperOre] -= 2;
                    return true;
                }
                break;
            case 18: //Pylon
                if (resources[res.CopperOre] - 1 >= 0)
                {
                    resources[res.CopperOre] -= 1;
                    return true;
                }
                break;
        }
        return true;
    }

    public void Increase(int typeOfWork, int amount = 1)
    {

        if (typeOfWork < res.Steel && resources[res.Energy]-1 >= 0)
        {
            if (typeOfWork == res.UraniumOre && resources[res.Energy] - 2 >= 0) resources[res.Energy] -= 1;
            else return;
            resources[res.Energy]--;
            resources[typeOfWork] += amount;
        }
        else if (typeOfWork == 18) resources[res.Energy]+=amount;
        else
        {
            switch (typeOfWork)
            {
                case res.Steel: //Steel Production
                    if (resources[res.IronOre] - amount >= 0
                        && resources[res.Energy] - 2 >= 0)
                    {
                        resources[res.IronOre] -= amount;
                        resources[res.Steel]++;
                    }

                    break;
                case res.Ammo: //Shells
                    if (resources[res.Steel] - amount >= 0
                        && resources[res.Energy] - 1 >= 0)
                    {
                        resources[res.Steel] -= amount;
                        resources[res.Ammo]++;
                    }

                    break;
                case res.Nuke: //Nukes
                    if (resources[res.UraniumOre] - amount >= 0 &&
                        resources[res.Steel] - 2 >= 0 && //How much Steel 
                        resources[res.Ammo] - 1 >= 0 &&
                        resources[res.Energy] - 3 >= 0) //How many shells
                    {
                        resources[res.UraniumOre] -= amount;
                        resources[res.Steel] -= 2;
                        resources[res.Ammo]--;
                        resources[res.Ammo]++;
                    }

                    break;

            }
        }
    }

    public void Research()
    {
        if (resources[res.Energy] - 2 >= 0)
        {
            resources[res.Energy] -= 2;
            resources[res.Research]++;
            
        }
    }

    public bool DecreaseIfEnough(int resource, int amount = 1)
    {
        if (test) return true;
        if (resources[resource] - amount >= 0)
        {
            resources[resource] -= amount;
            return true;
        }
        return false;
    }

    public void EndTurn()
    {
        howMuchOfEachResource[0].SetText(resources[res.CopperOre].ToString()); 
        howMuchOfEachResource[1].SetText(resources[res.IronOre].ToString());
        howMuchOfEachResource[2].SetText(resources[res.UraniumOre].ToString());
        howMuchOfEachResource[3].SetText(resources[res.Steel].ToString());
        howMuchOfEachResource[4].SetText(resources[res.Ammo].ToString());
        howMuchOfEachResource[5].SetText(resources[res.Nuke].ToString());
        howMuchOfEachResource[6].SetText(resources[res.Research].ToString());
        howMuchOfEachResource[7].SetText(resources[res.Energy].ToString());
    }
}
