using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : Singleton<DragManager>
{
    [SerializeField]
    private float minLockOnDistance = 250f;

    [HideInInspector]
    public List<GridContainer> currentlyActiveContainers = new List<GridContainer>();

    public Grid lastGrid = null;

    public void BeginDrag(IIsStorable storable)
    {
        //Save last known grid
        lastGrid = storable.GetCurrentStorage();
        //Set parent
        storable.GetTransform().SetParent(UIManager.Instance.viewableTransform, true);
    }

    public void UpdateDrag(IIsStorable storable)
    {
        //Update position
        storable.SetPosition(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
    }

    public void EndDrag(IIsStorable storable)
    {
        float lowest_distance = minLockOnDistance;
        Grid best_grid = null;

        //Search through all grids
        for (int i = 0; i < currentlyActiveContainers.Count; ++i)
        {
            GridContainer current_container = currentlyActiveContainers[i];
            for (int j = 0; j < current_container.allGrids.Count; ++j)
            {
                Grid current_grid = current_container.allGrids[j];
                float distance = Vector2.Distance(current_grid.transform.position, Input.mousePosition);
                if (distance < lowest_distance)
                {
                    //Check if found grid has a storable, and if the two storables can be combined
                    if (current_grid.isVacant || (!current_grid.isVacant && current_grid.storedObject.CanAcceptFood()))
                    {
                        lowest_distance = distance;
                        best_grid = current_grid;
                    }
                }
            }
        }

        if (best_grid == null)
        {
            //Did not find grid, return to last location
            storable.SetStorage(lastGrid);
        }
        else
        {
            //Found best grid
            storable.SetStorage(best_grid);
        }

        //Remove reference to last grid
        lastGrid = null;
    }
}
