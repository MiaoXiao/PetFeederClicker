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

    private List<IngredientPrepration> SetUpIngredientList(GameObject food_collection)
    {
        //create list of ingredients
        List<IngredientPrepration> ingredient_list = new List<IngredientPrepration>();
        for (int i = 0; i < food_collection.transform.childCount; ++i)
        {
            ingredient_list.Add(food_collection.transform.GetChild(i).GetComponent<Food>().originalIngredient);
            ingredient_list[ingredient_list.Count - 1].hasBeenChecked = false;
            for (int j = 0; j < food_collection.transform.GetChild(i).GetComponent<Food>().otherIngredients.Count; ++j)
            {
                ingredient_list.Add(food_collection.transform.GetChild(i).GetComponent<Food>().otherIngredients[j]);
                ingredient_list[ingredient_list.Count - 1].hasBeenChecked = false;
            }
        }
        return ingredient_list;
    }

    public void CheckValidRecipe(GameObject food_collection, bool pot)
    {
        //Compare with current recipes
        bool recipe_found = false;
        for(int i = 0; i < shownRecipe.allGrids.Count && !recipe_found; ++i)
        {
            List<IngredientPrepration> ingredient_list = SetUpIngredientList(food_collection);
            bool recipe_check_failed = false;
            RecipeHandler recipe_handler = shownRecipe.allGrids[i].transform.GetChild(0).GetComponent<RecipeHandler>();
            if ((recipe_handler.recipeData.canUsePot == pot || recipe_handler.recipeData.canUsePan == !pot) && !recipe_handler.Completed)
            {
                for (int j = 0; j < recipe_handler.recipeData.recipeList.Count && !recipe_check_failed; ++j)
                {
                    bool correct_ingredient_found = false;
                    for(int k = 0; k < ingredient_list.Count && !correct_ingredient_found; ++k)
                    {
                        print("comparing " + recipe_handler.recipeData.recipeList[j].ingredientToAddToRecipe.name + " to " + ingredient_list[k].Ingredient.name);
                        //Try to find ingredient match from recipe to ingredeint list
                        if (recipe_handler.recipeData.recipeList[j].ingredientToAddToRecipe.name == ingredient_list[k].Ingredient.name &&
                            recipe_handler.recipeData.recipeList[j].mustBeChopped == ingredient_list[k].fullyCut)
                        {
                            correct_ingredient_found = true;
                            ingredient_list[k].hasBeenChecked = true;
                            break;
                        }
                    }

                    if (!correct_ingredient_found)
                    {
                        recipe_check_failed = true;
                        print("incorrect match with recipe " + recipe_handler.name);
                    }

                }

                print(recipe_handler.totalIngredientsNeeded + " == " + ingredient_list.Count);
                if (!recipe_check_failed && recipe_handler.totalIngredientsNeeded == ingredient_list.Count)
                {
                    print("Recipe Match found with " + recipe_handler.name);
                    GameManager.Instance.currentScore += recipe_handler.recipeData.Points;
                    UIManager.Instance.SetScore(GameManager.Instance.currentScore);
                    recipe_handler.Completed = true;
                    recipe_found = true;

                }
            }
            else
            {
                print("incorrect cooking ware with " + recipe_handler.name);
            }
        }

        //Score discard bonus if neccesary
        if (!recipe_found)
        {
            int cut_ingre = 0;
            int uncut_ingre = 0;
            List<IngredientPrepration> ingre_prep = SetUpIngredientList(food_collection);
            for (int i = 0; i < ingre_prep.Count; ++i)
            {
                if (ingre_prep[i].fullyCut)
                    cut_ingre++;
                else
                    uncut_ingre++;
            }

            int discard_bonus = (uncut_ingre * GameManager.Instance.discardBonus) + (cut_ingre * GameManager.Instance.discardBonus * 2);
            GameManager.Instance.currentScore += discard_bonus;
            UIManager.Instance.SetScore(GameManager.Instance.currentScore);
        }

        //Discard contents
        for (int i = 0; i < food_collection.transform.childCount; ++i)
        {
            print("trashing " + food_collection.transform.GetChild(i).gameObject.name);
            food_collection.transform.GetChild(i).gameObject.SetActive(false);
        }
        food_collection.transform.DetachChildren();
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
