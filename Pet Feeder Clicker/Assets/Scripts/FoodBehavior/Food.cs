using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    private List<IngredientPreperation> ingredientCombination = new List<IngredientPreperation>();

    /// <summary>
    /// Number of ingredients in this food.
    /// </summary>
    public int GetNumberOfCurrentIngredients
    {
        get { return ingredientCombination.Count; }
    }
}

[Serializable]
public class IngredientPreperation
{
    public IngredientData ingredientData;

    private int _timesCut = 0;
    public int timesCut
    {
        get { return _timesCut; }
        set
        {
            if (timesCut < 0 || timesCut >= ingredientData.cutTimeInClicks)
                return;

            //TODO: change image to match number of times to cut
        }
    }

}
