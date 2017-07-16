using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Create New Ingredient")]
public class IngredientData : ScriptableObject
{
    public Sprite ingredientSprite;
    public Sprite cutIngredientSprite;

    [Space(10)]

    public Color fullyCookedTint = Color.gray;
}
