using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManger : MonoBehaviour
{
    public LayerMask SelectedItemsLayer;
    private SelectableItem selectedItem;
    private SelectableItem hoveringItem;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // Reset hovering item every frame
        if (hoveringItem)
        {
            hoveringItem.HoveringOver = false;
            hoveringItem = null;
        }
        // Check for selectable items under the cursor
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
        Collider2D collider = Physics2D.OverlapPoint(worldPoint2d, SelectedItemsLayer);
        if (collider)
        {
            SelectableItem item = collider.GetComponent<SelectableItem>();
            if (item)
            {
                // Set item to hovering if found
                item.HoveringOver = true;
                hoveringItem = item;
            }
        }
    }

    public void Update()
    {
        if (Input.GetButtonDown("MouseLeftClick"))
        {
            SelectableItem previousSelected = selectedItem;
            if (selectedItem)
            {
                selectedItem.Selected = false;
                selectedItem.OnUnselect();
                selectedItem = null;
            }
            if (hoveringItem)
            {
                if (previousSelected == hoveringItem)
                {
                    hoveringItem.OnSelect();
                }
                hoveringItem.Selected = true;
                hoveringItem.OnClick();
                selectedItem = hoveringItem;
            }
        }
    }
}
