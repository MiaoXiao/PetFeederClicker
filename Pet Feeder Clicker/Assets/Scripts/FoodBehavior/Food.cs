using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Food : MonoBehaviour, IIsStorable, IPointerDownHandler
{
    public IngredientPrepration firstIngredient { get { return allIngredients[0]; } }
    public List<IngredientPrepration> allIngredients = new List<IngredientPrepration>();

    public void AddFood(List<IngredientPrepration> other)
    {
        for (int i = 0; i < other.Count; ++i)
        {
            allIngredients.Add(other[i]);
            GameObject ingredient = GameManager.Instance.extraFoodPooler.GetObject();
            ingredient.transform.SetParent(transform, true);
            ingredient.transform.position = transform.position;
            ingredient.transform.localScale = transform.localScale;
            ingredient.GetComponent<Image>().sprite = other[i].Ingredient.cutIngredientSprite;
        }
    }

    private Image currentImage;

    /// <summary>
    /// Number of OTHER ingredients in this food.
    /// </summary>
    public int GetNumberOfExtraIngredients
    {
        get { return allIngredients.Count; }
    }

    private void Awake()
    {
        currentImage = GetComponent<Image>();
        currentImage.sprite = firstIngredient.Ingredient.ingredientSprite;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;
        
        if (allIngredients.Count > 1)
        {
            allIngredients.RemoveRange(1, allIngredients.Count - 1);
        }

        firstIngredient.numberOfCuts = 0;
        currentImage.sprite = firstIngredient.Ingredient.ingredientSprite;

        GetComponent<Image>().raycastTarget = true;
    }

    //////////////////////////////////////////
    //Interfaces
    //////////////////////////////////////////

    public bool CanAcceptFood()
    {
        return firstIngredient.fullyCut;
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

        if (new_grid.IsWindow() && firstIngredient.fullyCut)
        {
            //Check if scoring
            RecipeRandomizer.Instance.CheckValidRecipe(gameObject, false, true, false);
        }

        bool terminate_obj = false;
        if (new_grid.transform.childCount >= 2 &&
            GetComponent<IIsStorable>().CanAcceptFood() &&
            !new_grid.isVacant &&
            new_grid.storedObject.CanAcceptFood())
        {
            //Combine ingredients into one ingredient
            new_grid.storedObject.GetTransform().GetComponent<Food>().AddFood(allIngredients);

            print("combine");

            //Remove this object
            terminate_obj = true;
        }

        new_grid.AddItem();

        if (terminate_obj)
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                transform.GetChild(i).gameObject.SetActive(false);
                transform.GetChild(i).transform.SetParent(UIManager.Instance.foodTransform, false);
            }
            gameObject.SetActive(false);
            transform.SetParent(UIManager.Instance.foodTransform, true);

        }

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
            if (GetCurrentStorage().gridContainerParent is CuttingBoard && firstIngredient.Ingredient.canBeCut)
            {
                if (firstIngredient.numberOfCuts < 5)
                    AudioMana.Instance.PlayChoppoing();

                firstIngredient.numberOfCuts++;
                currentImage.sprite = firstIngredient.currentSprite;

                //Display particles


                //TODO: show cutting particles

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
        }
    }
    public bool hasBeenChecked = false;
    public Sprite currentSprite;

    public IngredientPrepration Clone()
    {
        IngredientPrepration new_prep = new IngredientPrepration();
        new_prep._numberOfCuts = _numberOfCuts;
        new_prep.currentSprite = currentSprite;
        new_prep.Ingredient = Ingredient;
        new_prep.hasBeenChecked = false;
        return new_prep;
    }
}