using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButton : MonoBehaviour
{
    public string title;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void Activate(bool active)
    {
        gameObject.SetActive(active);
    }
}
