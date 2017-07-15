using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Create New Ingredient")]
public class IngredientData : ScriptableObject
{
    public Image ingredientImage;
    public List<Image> cutIngredientImages;

    [Space(10)]

    public Color fullyCookedTint = Color.gray;

    public int cutTimeInClicks
    {
        get { return cutIngredientImages.Count; }
    }
}
