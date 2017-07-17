using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableCookingWare : MonoBehaviour
{
    private Vector3 lastPosition;
    private Transform lastParent;

    public bool isPot = false;

    [SerializeField]
    private CookingWare cookingWare;

    [SerializeField]
    private WindowGrid windowGrid;

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
        print("end");
        print(windowGrid.isHoveredOver);
        //TODO: Check for possible trashing or scoring food
        if (windowGrid.isHoveredOver && cookingWare.finishedCooking)
        {
            print("attempt score");
            if (cookingWare.transform.childCount != 0)
            {
                RecipeRandomizer.Instance.CheckValidRecipe(cookingWare.gameObject, isPot);
            }
            else
            {
                print("empty");
            }
            //Reset cookingware
            cookingWare.Reset();

        }

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
