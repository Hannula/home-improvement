﻿using System.Collections;
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
    public SpriteRenderer IsWeaponRenderer;

    public Color SelectedColor;
    public Color EmptyColor;
    public Color HoverColor;
    public Color FilledColor;

    private InventoryManager inventoryManager;
    private bool previouslyHovered;
    private bool hidden;

    private AudioSource buildSound;

    public void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        UpdateInventory();
    }

    public bool HideIfEmpty()
    {
        if (IsEmpty)
        {
            EmptyColor = Color.clear;
            Update();
            hidden = true;
            return true;
        }

        return false;
    }

    public virtual void Update()
    {
        if (hidden)
        {
            return;
        }

        IsWeaponRenderer.enabled = false;
        if (HomeUpgrade != null && HomeUpgrade.UpgradeData != null)
        {
            ImageSpriteRenderer.sprite = HomeUpgrade.UpgradeData.Image;

            ImageSpriteRenderer.color = HomeUpgrade.GetColor();
            if (ImageSpriteRenderer.color.a == 0)
            {
                ImageSpriteRenderer.color = Color.white;
            }
            if (HomeUpgrade.UpgradeData.IsWeapon )
            {
                IsWeaponRenderer.enabled = true;
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

    public void UpdateInventory()
    {
        if (ParentFloor == null)
        {
            HomeUpgrade = GameManager.Instance.Inventory[InventoryIndex];
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

                buildSound = SFXPlayer.Instance.Play(Sound.Repair, volumeFactor: 0.5f);
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
        if (inventoryManager && HomeUpgrade != null)
        {
            inventoryManager.SetItemInfo("<color=" + HomeUpgrade.ModifierData.GetRarityColor() + ">" + HomeUpgrade.GetName() + "</color>", HomeUpgrade.GetDescription());
        }
    }

    public override void OnClick()
    {
        if (GameManager.Instance.State != GameManager.GameState.Battle)
        {
            HomeUpgrade newUpgrade = inventoryManager.FloatingUpgrade;
            inventoryManager.FloatingUpgrade = Remove();
            Insert(newUpgrade);
        }

    }

}
