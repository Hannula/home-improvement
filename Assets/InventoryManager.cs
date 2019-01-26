using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public bool ShowInfo;
    public Text InventoryItemName;
    public Text InventoryItemDescription;


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
}
