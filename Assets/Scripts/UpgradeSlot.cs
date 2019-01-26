using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : SelectableItem
{
    public bool IsEmpty { get { return HomeUpgrade == null; } }
    public HomeUpgrade HomeUpgrade;
    public SpriteRenderer BackgroundSpriteRenderer;
    public SpriteRenderer ImageSpriteRenderer;

    public Color SelectedColor;
    public Color EmptyColor;
    public Color HoverColor;
    public Color FilledColor;

    public void Update()
    {
        if (HomeUpgrade != null && HomeUpgrade.UpgradeData != null)
        {
            ImageSpriteRenderer.sprite = HomeUpgrade.UpgradeData.Image;
        }

        if (IsEmpty)
        {
            BackgroundSpriteRenderer.color = EmptyColor;
        }
        if (!IsEmpty)
        {
            BackgroundSpriteRenderer.color = FilledColor;
        }
        if (HoveringOver)
        {
            BackgroundSpriteRenderer.color = HoverColor;
        }
        if (Selected)
        {
            BackgroundSpriteRenderer.color = SelectedColor;
        }
    }

}
