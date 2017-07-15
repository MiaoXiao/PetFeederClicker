﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContainer : MonoBehaviour
{
    [HideInInspector]
    public List<Grid> allGrids = new List<Grid>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            allGrids.Add(transform.GetChild(i).GetComponent<Grid>());
        }
    }

    private void OnEnable()
    {
        DragManager.Instance.currentlyActiveContainers.Add(this);
    }

    private void OnDisable()
    {
        DragManager.Instance.currentlyActiveContainers.Remove(this);
    }
}
