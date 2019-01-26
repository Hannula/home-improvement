using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableItem : MonoBehaviour
{
    
    public bool Selected;
    public bool HoveringOver;

    // Everytime item is clicked
    public virtual void OnClick()
    {

    }

    // Only when selected the first time
    public virtual void OnSelect()
    {

    }

    public virtual void OnUnselect()
    {

    }
}
