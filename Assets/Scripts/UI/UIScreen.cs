using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    public bool active;

    public virtual void Activate(bool active)
    {
        if (this.active != active)
        {
            this.active = active;
            gameObject.SetActive(active);
        }
    }
}
