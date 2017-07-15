﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool unlimitedStorage = false;

    public bool boundsIsFullImage = false;

    public bool isHoveredOver = false;

    public IIsStorable storedObject
    {
        get { return transform.GetChild(0).GetComponent<IIsStorable>(); }
    }

    //private GridContainer gridContainerParent;

    public bool isVacant
    {
        get { return transform.childCount == 0; }
    }

    private void Awake()
    {
        //gridContainerParent = GetComponentInParent<GridContainer>();
    }

    public void PointerEnter()
    {
        isHoveredOver = true;
    }

    public void PointerExit()
    {
        isHoveredOver = false;
    }
}
