using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableCookingWare : MonoBehaviour
{
    private Vector3 lastPosition;
    private Transform lastParent;

    [SerializeField]
    private CookingWare cookingWare;

    public void BeginDrag()
    {
        cookingWare.Paused = true;
        lastPosition = transform.position;
        lastParent = transform.parent.transform;
        transform.SetParent(UIManager.Instance.viewableTransform, true);
        transform.position = Input.mousePosition;

        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; ++i)
        {
            images[i].raycastTarget = false;
        }
    }

    public void UpdateDrag()
    {
        transform.position = Input.mousePosition;
        //DragManager.Instance.UpdateDrag(GetComponent<IIsStorable>());
    }

    public void EndDrag()
    {
        //TODO: Check for possible trashing or scoring food

        //Restore original position
        transform.position = lastPosition;
        transform.SetParent(lastParent, true);
        cookingWare.Paused = false;
        //DragManager.Instance.EndDrag(GetComponent<IIsStorable>());

        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; ++i)
        {
            images[i].raycastTarget = true;
        }
    }
}
