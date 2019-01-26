using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSlot : SelectableItem
{
    public bool IsEmpty { get { return HomeUpgrade == null; } }
    public Floor ParentFloor;
    public int ParentFloorSlotIndex;
    public int InventoryIndex;
    public HomeUpgrade HomeUpgrade;
    public SpriteRenderer BackgroundSpriteRenderer;
    public SpriteRenderer ImageSpriteRenderer;

    public Color SelectedColor;
    public Color EmptyColor;
    public Color HoverColor;
    public Color FilledColor;

    private bool previouslyHovered;
    private InventoryManager inventoryManager;

    private AudioSource buildSound;

    public void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void Update()
    {
        if (HomeUpgrade != null && HomeUpgrade.UpgradeData != null)
        {
            ImageSpriteRenderer.sprite = HomeUpgrade.UpgradeData.Image;

            ImageSpriteRenderer.color = HomeUpgrade.GetColor();
            if (ImageSpriteRenderer.color.a == 0)
            {
                ImageSpriteRenderer.color = Color.white;
            }
        }
        else
        {
            ImageSpriteRenderer.sprite = null;
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

    // Insert new upgrade to the slot
    public HomeUpgrade Insert(HomeUpgrade upgrade)
    {
        HomeUpgrade previousUpgrade = HomeUpgrade;
        HomeUpgrade = upgrade;
        if (ParentFloor != null)
        {
            ParentFloor.FloorData.HomeUpgrades[ParentFloorSlotIndex] = upgrade;
            if (upgrade != null)
            {
                if (buildSound != null && buildSound.isPlaying)
                {
                    buildSound.Stop();
                }

                buildSound = SFXPlayer.Instance.Play(Sound.Repair);
            }
        }
        else
        {
            GameManager.Instance.Inventory[InventoryIndex] = upgrade;
        }
        return previousUpgrade;
    }

    // Pop the item out
    public HomeUpgrade Remove()
    {
        HomeUpgrade previousUpgrade = HomeUpgrade;
        HomeUpgrade = null;
        if (ParentFloor != null)
        {
            ParentFloor.FloorData.HomeUpgrades[ParentFloorSlotIndex] = null;
        }
        else
        {
            GameManager.Instance.Inventory[InventoryIndex] = null;
        }
        return previousUpgrade;
    }

    public void ShowInfo()
    {
        if (GameManager.Instance.State != GameManager.GameState.Battle && inventoryManager && HomeUpgrade != null)
        {
            inventoryManager.SetItemInfo(HomeUpgrade.GetName(), HomeUpgrade.GetDescription());
        }
    }

    public override void OnClick()
    {
        HomeUpgrade newUpgrade = inventoryManager.FloatingUpgrade;
        inventoryManager.FloatingUpgrade = Remove();
        Insert(newUpgrade);

    }

}
