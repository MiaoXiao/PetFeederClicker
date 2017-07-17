using System.Collections;
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

    public GridContainer gridContainerParent { get { return GetComponentInParent<GridContainer>(); } }

    public bool isVacant
    {
        get { return transform.childCount == 0; }
    }

    public void PointerEnter()
    {
        isHoveredOver = true;
    }

    public void PointerExit()
    {
        isHoveredOver = false;
    }

    public virtual void AddItem() { }
    public virtual bool IsWindow() { return false; }
}
