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

    private bool previouslyHovered;
    private InventoryManager inventoryManager;

    public void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

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
            if (previouslyHovered == false)
            {
                ShowInfo();
            }
            BackgroundSpriteRenderer.color = HoverColor;
            previouslyHovered = true;
        }
        else
        {
            previouslyHovered = false;
        }
        if (Selected)
        {
            BackgroundSpriteRenderer.color = SelectedColor;
        }

    }

    public void ShowInfo()
    {
        if (inventoryManager && HomeUpgrade != null)
        {
            inventoryManager.SetItemInfo(HomeUpgrade.GetName(), HomeUpgrade.GetDescription());
        }
    }

}
