using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeHandler : MonoBehaviour, IPointerDownHandler
{
    public RecipeData recipeData;

    private bool _Completed = false;
    public bool Completed
    {
        get { return _Completed; }
        set
        {
            if (value == _Completed)
                return;

            if (value)
            {
                transform.parent.GetComponent<Image>().color = Color.green;
            }
            else
            {
                transform.parent.GetComponent<Image>().color = Color.white;
            }

            _Completed = value;
        }
    }

    public int totalIngredientsNeeded
    {
        get
        {
            int total = 0;
            for (int i = 0; i < recipeData.recipeList.Count; ++i)
            {
                total += recipeData.recipeList[i].Amount;
            }
            return total;
        }
    }

    [SerializeField]
    private GameObject recipeRequirements;

    [SerializeField]
    private GameObject potObj;

    [SerializeField]
    private GameObject panObj;

    [SerializeField]
    private Text textPoints;

    public void Awake()
    {
        InitRecipeDisplay();
    }

    private void InitRecipeDisplay()
    {
        potObj.SetActive(recipeData.canUsePot);
        panObj.SetActive(recipeData.canUsePan);

        textPoints.text = recipeData.Points.ToString();

        int i = 0;
        while(i < recipeData.recipeList.Count)
        {
            if (recipeData.recipeList[i].mustBeChopped)
            {
                recipeRequirements.transform.GetChild(i).GetComponent<Image>().sprite = recipeData.recipeList[i].ingredientToAddToRecipe.cutIngredientSprite;
            }
            else
            {
                recipeRequirements.transform.GetChild(i).GetComponent<Image>().sprite = recipeData.recipeList[i].ingredientToAddToRecipe.ingredientSprite;
            }
            ++i;
        }

        while(i < 6)
        {
            recipeRequirements.transform.GetChild(i).gameObject.SetActive(false);
            ++i;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            RecipeRandomizer.Instance.RemoveRecipe(transform.parent.GetSiblingIndex());
            if (Completed)
            {
                Completed = false;
            }
            else
            {
                GameManager.Instance.currentScore -= GameManager.Instance.scorePenalty;
                UIManager.Instance.SetScore(GameManager.Instance.currentScore);
            }
        }
    }
}
