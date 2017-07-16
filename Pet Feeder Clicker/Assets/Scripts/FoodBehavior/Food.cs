using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Food : MonoBehaviour, IIsStorable, IPointerDownHandler
{
    public IngredientPrepration originalIngredient;

    public List<IngredientPrepration> otherIngredients = new List<IngredientPrepration>();

    public void AddFood(List<IngredientPrepration> other)
    {
        for (int i = 0; i < other.Count; ++i)
        {
            otherIngredients.Add(other[i].Clone());
            GameObject ingredient = GameManager.Instance.extraFoodPooler.GetObject();
            ingredient.transform.SetParent(transform, true);
            ingredient.transform.position = transform.position;
            ingredient.GetComponent<Image>().sprite = other[i].currentSprite;
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
        currentImage.sprite = originalIngredient.Ingredient.ingredientSprite;
    }

    //////////////////////////////////////////
    //Interfaces
    //////////////////////////////////////////

    public bool CanAcceptFood()
    {
        return originalIngredient.fullyCut;
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

        if (new_grid.transform.childCount >= 2 &&
            GetComponent<IIsStorable>().CanAcceptFood() &&
            !new_grid.isVacant &&
            new_grid.storedObject.CanAcceptFood() &&
            ((CookingWare)new_grid) == null)
        {

            print("combine");

            //Combine ingredients into one ingredient
            otherIngredients.Add(originalIngredient);
            new_grid.storedObject.GetTransform().GetComponent<Food>().AddFood(otherIngredients);

            //Remove this object
            transform.SetParent(null, true);
            gameObject.SetActive(false);
        }

        new_grid.AddItem();
        
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
            //print("click");
            if (GetCurrentStorage().gridContainerParent is CuttingBoard)
            {
                originalIngredient.numberOfCuts++;
                currentImage.sprite = originalIngredient.currentSprite;
            }

        }
    }
}

[Serializable]
public class IngredientPrepration
{
    public IngredientData Ingredient;

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
                currentSprite = Ingredient.cutIngredientSprite;
            }
            else
            {
                _numberOfCuts = value;
                currentSprite = Ingredient.ingredientSprite;
            }

            //TODO: show cutting particles
        }
    }

    public Sprite currentSprite;

    public IngredientPrepration Clone()
    {
        IngredientPrepration new_prep = new IngredientPrepration();
        new_prep._numberOfCuts = _numberOfCuts;
        new_prep.currentSprite = currentSprite;
        new_prep.Ingredient = Ingredient;
        return new_prep;
    }
}