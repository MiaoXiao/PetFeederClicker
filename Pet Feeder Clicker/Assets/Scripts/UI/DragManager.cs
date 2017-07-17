using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragManager : Singleton<DragManager>
{
    [SerializeField]
    private float minLockOnDistance = 250f;

    [HideInInspector]
    public List<GridContainer> currentlyActiveContainers = new List<GridContainer>();

    public Grid lastGrid = null;
    public Vector3 lastPosition = Vector3.zero;

    public Grid hoveredGrid = null;

    public void BeginDrag(IIsStorable storable)
    {
        AudioMana.Instance.PlayPickUp();
        //print("begin");
        //Save last known grid
        lastGrid = storable.GetCurrentStorage();
        //Set parent
        storable.GetTransform().SetParent(UIManager.Instance.viewableTransform, true);
        //Turn off raycast
        if (storable.GetTransform().GetComponent<Image>() != null)
            storable.GetTransform().GetComponent<Image>().raycastTarget = false;

        lastPosition = storable.GetTransform().position;
    }

    public void UpdateDrag(IIsStorable storable)
    {
        //print("update");
        //Update position
        storable.SetPosition(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
    }
    int x = 0;
    public void EndDrag(IIsStorable storable)
    {
        x++;

        //print("end");
        float lowest_distance = minLockOnDistance;
        Grid best_grid = null;
        bool done_searching = false;
        //Search through all grids
        for (int i = 0; i < currentlyActiveContainers.Count && !done_searching; ++i)
        {
            GridContainer current_container = currentlyActiveContainers[i];
            for (int j = 0; j < current_container.allGrids.Count; ++j)
            {

                Grid current_grid = current_container.allGrids[j];

                if (!current_container.allowedTypes.Contains(storable.GetTypeName()))
                    break;

                if (current_grid.boundsIsFullImage && current_grid.isHoveredOver && current_grid.unlimitedStorage)
                {
                    best_grid = current_grid;
                    done_searching = true;
                    break;
                }

                float distance = Vector2.Distance(current_grid.transform.position, Input.mousePosition);
                if (distance < lowest_distance)
                {
                    //Check if found grid has a storable, and if the two storables can be combined
                    if (current_grid.unlimitedStorage || current_grid.isVacant || (!current_grid.isVacant && current_grid.storedObject.CanAcceptFood() && storable.CanAcceptFood()))
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
            storable.GetTransform().position = lastPosition;
        }
        else
        {
            //if (x < 7)
            //{
                //Found best grid
                storable.SetStorage(best_grid);
            //}
        }

        //Remove reference to last grid
        lastGrid = null;

        //Turn on raycast
        storable.GetTransform().GetComponent<Image>().raycastTarget = true;
    }
}
