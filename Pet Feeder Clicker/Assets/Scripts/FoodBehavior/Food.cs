using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Food : MonoBehaviour, IIsStorable, IPointerDownHandler
{
    public IngredientData originalIngredient;

    [SerializeField]
    private List<IngredientData> otherIngredients = new List<IngredientData>();

    public bool fullyCut { get { return _numberOfCuts == GameManager.Instance.totalCutsPerIngredient; } }

    private int _numberOfCuts = 0;
    public int numberOfCuts
    {
        get { return _numberOfCuts; }
        set
        {
            if (value > GameManager.Instance.totalCutsPerIngredient)
                return;

            if (value == GameManager.Instance.totalCutsPerIngredient)
            {
                _numberOfCuts = GameManager.Instance.totalCutsPerIngredient;
                currentImage.sprite = originalIngredient.cutIngredientSprite;
            }
            else
            {
                _numberOfCuts = value;
            }

            //TODO: show cutting particles
        }
    }

    private Image currentImage;

    /// <summary>
    /// Number of OTHER ingredients in this food.
    /// </summary>
    public int GetNumberOfExtraIngredients
    {
        get { return otherIngredients.Count; }
    }

    private void Awake()
    {
        currentImage = GetComponent<Image>();
        currentImage.sprite = originalIngredient.ingredientSprite;
    }

    //////////////////////////////////////////
    //Interfaces
    //////////////////////////////////////////

    public bool CanAcceptFood()
    {
        return false;
    }

    public Grid GetCurrentStorage()
    {
        return GetComponentInParent<Grid>();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetStorage(Grid new_grid)
    {
        //print("moving " + transform.name + " to " + new_grid.name);
        transform.SetParent(new_grid.transform, true);
    }

    public string GetTypeName()
    {
        return GetType().Name;
    }

    //////////////////////////////////////////
    //Drag Events
    //////////////////////////////////////////

    public void BeginDrag()
    {
        DragManager.Instance.BeginDrag(GetComponent<IIsStorable>());
    }

    public void UpdateDrag()
    {
        DragManager.Instance.UpdateDrag(GetComponent<IIsStorable>());
    }

    public void EndDrag()
    {
        DragManager.Instance.EndDrag(GetComponent<IIsStorable>());
    }

    //////////////////////////////////////////
    //Click Events
    //////////////////////////////////////////

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (GetComponent<IIsStorable>().GetCurrentStorage().gridContainerParent is CuttingBoard)
            numberOfCuts++;
        }
    }
}
