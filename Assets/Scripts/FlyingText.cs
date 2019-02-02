using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingText : MonoBehaviour
{
    public float lifeTime = 1f;
    public float speed = 0.01f;
    public Vector3 flyDirection = Vector3.up;
    public bool fadeOut = true;
    public bool active;

    private Text text;
    private float elapsedTime;

    // Start is called before the first frame update
    private void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.Instance.GamePaused
            && active)
        {
            elapsedTime += Time.deltaTime;

            //Move();
            if (fadeOut)
            {
                FadeOut();
            }

            if (elapsedTime >= lifeTime)
            {
                Disappear();
            }
        }
    }

    public void Activate(string str, Vector3 startPosition, float speed, Color color)
    {
        text.text = str;
        text.color = color;
        transform.position = startPosition;
        this.speed = speed;
        active = true;
    }

    public void Activate(string str, Color color)
    {
        Activate(str, transform.position, speed, color);
    }

    private void Move()
    {
        // TODO: Fix. It doesn't work because the object
        // is an UI element, not a normal game object
        Vector3 newPosition = (flyDirection - transform.position).normalized * speed;
        transform.position = newPosition;
    }

    private void FadeOut()
    {
        if (lifeTime > 0f)
        {
            Color newColor = text.color;
            newColor.a = 1f - (elapsedTime / lifeTime);
            text.color = newColor;
        }
    }

    private void Disappear()
    {
        text.text = "";
        active = false;
    }

    private void ResetValues()
    {
        elapsedTime = 0;
        text.text = "";
        active = false;
    }
}
