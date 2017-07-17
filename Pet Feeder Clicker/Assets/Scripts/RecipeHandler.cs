using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeHandler : MonoBehaviour
{
    public RecipeData recipeData;

    public bool Completed = false;

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
}
