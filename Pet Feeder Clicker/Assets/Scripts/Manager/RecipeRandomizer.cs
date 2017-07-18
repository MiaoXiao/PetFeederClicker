using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeRandomizer : Singleton<RecipeRandomizer>
{
    [SerializeField]
    private List<RecipeHandler> allRecipes = new List<RecipeHandler>();

    [SerializeField]
    private GridContainer recipeGridContainer;

    private List<RecipeHandler> shownRecipes = new List<RecipeHandler>();

    public void StartGeneration()
    {
        //Generate 3
        for (int i = 0; i < 3; ++i)
        {
            GetNewRecipe(i);
        }
    }

    private List<IngredientPrepration> SetUpIngredientList(GameObject food_collection, bool special)
    {
        //create list of ingredients
        if (!special)
        {
            List<IngredientPrepration> ingredient_list = new List<IngredientPrepration>();
            for (int i = 0; i < food_collection.transform.childCount; ++i)
            {
                for (int j = 0; j < food_collection.transform.GetChild(i).GetComponent<Food>().allIngredients.Count; ++j)
                {
                    ingredient_list.Add(food_collection.transform.GetChild(i).GetComponent<Food>().allIngredients[j]);
                    ingredient_list[ingredient_list.Count - 1].hasBeenChecked = false;
                }
            }
            return ingredient_list;
        }
        else
        {
            List<IngredientPrepration> ingredient_list = new List<IngredientPrepration>();
            for (int i = 0; i < food_collection.transform.childCount; ++i)
            {
                for (int j = 0; j < food_collection.GetComponent<Food>().allIngredients.Count; ++j)
                {
                    ingredient_list.Add(food_collection.GetComponent<Food>().allIngredients[j]);
                    ingredient_list[ingredient_list.Count - 1].hasBeenChecked = false;
                }
            }
            return ingredient_list;
        }
    }

    public void CheckValidRecipe(GameObject food_collection, bool pot, bool special, bool is_burned)
    {
        bool recipe_found = false;
        if (!is_burned)
        {
            for (int i = 0; i < recipeGridContainer.allGrids.Count && !recipe_found; ++i)
            {
                List<IngredientPrepration> ingredient_list = SetUpIngredientList(food_collection, special);
                bool recipe_check_failed = false;
                RecipeHandler recipe_handler = recipeGridContainer.allGrids[i].transform.GetChild(0).GetComponent<RecipeHandler>();
                if ((recipe_handler.recipeData.canUsePot == pot || recipe_handler.recipeData.canUsePan == !pot) && !recipe_handler.Completed)
                {
                    for (int j = 0; j < recipe_handler.recipeData.recipeList.Count && !recipe_check_failed; ++j)
                    {
                        bool correct_ingredient_found = false;
                        for (int k = 0; k < ingredient_list.Count && !correct_ingredient_found; ++k)
                        {
                            print("comparing " + recipe_handler.recipeData.recipeList[j].ingredientToAddToRecipe.name + " to " + ingredient_list[k].Ingredient.name);
                            print(recipe_handler.recipeData.recipeList[j].mustBeChopped + "  vs " + ingredient_list[k].fullyCut);
                            //Try to find ingredient match from recipe to ingredeint list
                            if (recipe_handler.recipeData.recipeList[j].ingredientToAddToRecipe.name == ingredient_list[k].Ingredient.name &&
                                recipe_handler.recipeData.recipeList[j].mustBeChopped == ingredient_list[k].fullyCut)
                            {
                                print("found correct " + recipe_handler.recipeData.recipeList[j].ingredientToAddToRecipe.name);
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
        }
       

        //Score discard bonus if neccesary
        if (!recipe_found)
        {
            int cut_ingre = 0;
            int uncut_ingre = 0;
            List<IngredientPrepration> ingre_prep = SetUpIngredientList(food_collection, special);
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

        if (special)
        {
            food_collection.GetComponent<Food>().TerminateObject();
        }
        else
        {
            print(food_collection.name);
            for (int i = food_collection.transform.childCount - 1; i >= 0; --i)
            {
                print("trashing " + food_collection.transform.GetChild(i).gameObject.name);
                food_collection.transform.GetChild(i).gameObject.GetComponent<Food>().TerminateObject();
            }
        }
    }

    public void GetNewRecipe(int slot)
    {
        if (slot < 0 || slot >= 3)
            return;

        RecipeHandler add_back = null;
        //Detach recipe from slot if neccesary
        if (recipeGridContainer.allGrids[slot].transform.childCount != 0)
        {
            add_back = recipeGridContainer.allGrids[slot].transform.GetChild(0).GetComponent<RecipeHandler>();
            shownRecipes.Remove(add_back);
            recipeGridContainer.allGrids[slot].transform.GetChild(0).gameObject.SetActive(false);
            recipeGridContainer.allGrids[slot].transform.DetachChildren();
        }

        int index = Random.Range(0, allRecipes.Count);
        RecipeHandler recipe = allRecipes[index];

        recipe.transform.SetParent(recipeGridContainer.allGrids[slot].transform, true);
        recipe.transform.gameObject.SetActive(true);

        shownRecipes.Add(recipe);
        allRecipes.Remove(recipe);

        if (add_back != null)
        {
            allRecipes.Add(add_back);
        }

    }
}
