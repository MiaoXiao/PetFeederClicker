using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeRandomizer : Singleton<RecipeRandomizer>
{
    [SerializeField]
    private int numberOfInitialEasy = 4;
    private int generatedEasy = 0;

    [SerializeField]
    private List<RecipeHandler> allRecipes = new List<RecipeHandler>();

    private List<RecipeHandler> easyRecipes = new List<RecipeHandler>();
    private List<RecipeHandler> normalRecipes = new List<RecipeHandler>();

    [SerializeField]
    private GridContainer shownRecipe;

    public void StartGeneration()
    {
        InitEasyRecipes();
        InitNormalRecipes();

        //Generate 3
        for (int i = 0; i < 3; ++i)
        {
            GetNewRecipe(i, true);
        }
        generatedEasy = numberOfInitialEasy;
    }

    public void CheckValidRecipe(GameObject food_collection, bool pot)
    {
       //create list of ingredients
        List<IngredientPrepration> ingredient_list = new List<IngredientPrepration>();
        for (int i = 0; i < food_collection.transform.childCount; ++i)
        {
            ingredient_list.Add(food_collection.transform.GetChild(i).GetComponent<Food>().originalIngredient);
            for (int j = 0; j < food_collection.transform.GetChild(i).GetComponent<Food>().otherIngredients.Count; ++j)
            {
                ingredient_list.Add(food_collection.transform.GetChild(i).GetComponent<Food>().otherIngredients[j]);
            }
        }

        //Compare with current recipes
        for(int i = 0; i < shownRecipe.allGrids.Count; ++i)
        {
            RecipeHandler recipe_handler = shownRecipe.allGrids[i].transform.GetChild(0).GetComponent<RecipeHandler>();
            if (recipe_handler.recipeData.canUsePot == pot || recipe_handler.recipeData.canUsePan == !pot)
            {

            }
        }
    }

    public void RemoveRecipe(int slot)
    {
        if (generatedEasy == numberOfInitialEasy)
            GetNewRecipe(slot, false);
        else
        {
            GetNewRecipe(slot, true);
            generatedEasy++;
        }

    }

    private void GetNewRecipe(int slot, bool force_easy)
    {
        if (slot < 0 || slot >= 3)
            return;

        //Detach recipe from slot
        if (shownRecipe.allGrids[slot].transform.childCount != 0)
        {
            shownRecipe.allGrids[slot].transform.GetChild(0).gameObject.SetActive(false);
            shownRecipe.allGrids[slot].transform.DetachChildren();
        }

        int chooser = Random.Range(0, easyRecipes.Count + normalRecipes.Count + 1);
        if (force_easy || chooser <= easyRecipes.Count)
        {
            //Make easy
            int index = Random.Range(0, easyRecipes.Count);
            RecipeHandler recipe = easyRecipes[index];

            recipe.transform.SetParent(shownRecipe.allGrids[slot].transform, true);
            easyRecipes.Remove(recipe);

            if (easyRecipes.Count == 0)
                InitEasyRecipes();
        }
        else
        {
            //Make normal
            int index = Random.Range(0, normalRecipes.Count);
            RecipeHandler recipe = normalRecipes[index];

            recipe.transform.SetParent(shownRecipe.allGrids[slot].transform, true);
            normalRecipes.Remove(recipe);

            if (normalRecipes.Count == 0)
                InitNormalRecipes();
        }


    }

    private void InitEasyRecipes()
    {
        easyRecipes.Clear();
        for (int i = 0; i < allRecipes.Count; ++i)
        {
            if (allRecipes[i].recipeData.recipeType == RecipeType.Easy)
                easyRecipes.Add(allRecipes[i]);
        }
    }

    private void InitNormalRecipes()
    {
        normalRecipes.Clear();
        for (int i = 0; i < allRecipes.Count; ++i)
        {
            if (allRecipes[i].recipeData.recipeType == RecipeType.Normal)
                normalRecipes.Add(allRecipes[i]);
        }
    }
}
