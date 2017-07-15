using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Create New Recipe")]
public class RecipeData : ScriptableObject
{
    public RecipeType recipeType;
    public int Points = 0;

    [Space(10)]

    public Image recipeImage;
    public List<IngredientType> recipeList = new List<IngredientType>();

    [Space(10)]

    public bool canUsePot = true;
    public bool canUsePan = true;
}

[Serializable]
public class IngredientType
{
    public IngredientData ingredientToAddToRecipe;
    public int Amount = 1;
    public bool mustBeChopped = false;
}

public enum RecipeType
{
    Easy,
    Normal
}

