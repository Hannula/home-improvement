using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : SelectableItem
{
    public bool IsEmpty { get { return true; } }

    public SpriteRenderer SpriteRenderer;
    public Color SelectedColor;
    public Color EmptyColor;
    public Color HoverColor;
    public Color FilledColor;

    public void Update()
    {
        if (IsEmpty)
        {
            SpriteRenderer.color = EmptyColor;
        }
        if (!IsEmpty)
        {
            SpriteRenderer.color = FilledColor;
        }
        if (HoveringOver)
        {
            SpriteRenderer.color = HoverColor;
        }
        if (Selected)
        {
            SpriteRenderer.color = SelectedColor;
        }
    }

}
