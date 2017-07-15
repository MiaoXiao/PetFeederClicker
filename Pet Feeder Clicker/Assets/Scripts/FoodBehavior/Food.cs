using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IIsStorable
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

    public bool CanAcceptFood()
    {
        return false;
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
        print("moving " + transform.name + " to " + new_grid.name);
        transform.SetParent(new_grid.transform, true);
    }

    //Drag events
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
