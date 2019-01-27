using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMover : MonoBehaviour
{

    public float scrollSpeed = 0.5f;
    Renderer rend;
    private float offset;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        offset += GameManager.Instance.DeltaTime * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
