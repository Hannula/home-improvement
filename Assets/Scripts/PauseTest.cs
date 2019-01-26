using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTest : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 speed;
    public float maxDist;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position + speed * GameManager.Instance.DeltaTime;
        if (Vector3.Distance(newPos, startPos) > maxDist)
        {
            speed = -1 * speed;
        }
        else
        {
            transform.position = newPos;
        }
    }
}
