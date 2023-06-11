using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TileVisuals : MonoBehaviour
{
    [SerializeField] Sprite[] allSprites;

    public void Start()
    {

    }

    public Sprite GetSprite(int typeOfCell)
    {
        if (allSprites.Length < typeOfCell) return allSprites[0];
        return allSprites[typeOfCell];
    }
    
    
}
