using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public bool ShowInfo;
    public Text InventoryItemName;
    public Text InventoryItemDescription;
    public HomeUpgrade FloatingUpgrade;

    public SpriteRenderer FloatingItemRenderer;

    private UpgradeSlot[] upgradeSlots;

    private void Start()
    {
        upgradeSlots = FindObjectsOfType<UpgradeSlot>();
    }

    private void Update()
    {
        // Check for selectable items under the cursor
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
        FloatingItemRenderer.transform.position = worldPoint2d;
        if (FloatingUpgrade != null)
        {
            FloatingItemRenderer.sprite = FloatingUpgrade.UpgradeData.Image;
        }
        else
        {
            FloatingItemRenderer.sprite = null;
        }
    }

    public void DropFloatingUpgrade()
    {
        if (FloatingUpgrade != null)
        {
            GameManager.Instance.Inventory.Add(FloatingUpgrade);
            FloatingUpgrade = null;
        }
    }

    public void SetItemInfo(string name, string desc)
    {
        InventoryItemName.text = name;
        InventoryItemDescription.text = desc;
    }

    public void ResetItemInfo()
    {
        InventoryItemName.text = "";
        InventoryItemDescription.text = "";
    }

    public bool AddItem(HomeUpgrade itemToAdd)
    {
        List<HomeUpgrade> inventory = GameManager.Instance.Inventory;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = itemToAdd;
                UpdateUpgradeSlots();
                return true;
            }
        }

        Debug.LogWarning("Inventory full!");
        return false;
    }

    private void UpdateUpgradeSlots()
    {
        foreach (UpgradeSlot us in upgradeSlots)
        {
            us.UpdateInventory();
        }
    }
}
