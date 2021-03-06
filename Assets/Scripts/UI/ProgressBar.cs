﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float progress = 1f;

    public Color fullColor = new Color(0.13f, 0.73f, 0.14f);
    public Color notFullColor = new Color(0.72f, 0.14f, 0.14f);
    public float minScale;
    public float maxScale;
    public bool horizontal = true;
    public bool reverse;
    public Transform scaleTransform;
    public GameObject bar;

    private SpriteRenderer barSpriteRend;
    private Vector3 defaultScale;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Awake()
    {
        if (maxScale == 0)
        {
            maxScale = bar.transform.localScale.x;
        }
        barSpriteRend = bar.GetComponent<SpriteRenderer>();
        defaultScale = bar.transform.localScale;

        SetProgress(progress);
    }

    private Vector3 GetDifferenceFromStartToCurrPos()
    {
        if (horizontal)
        {
            return new Vector3((reverse ? -0.5f : 0.5f) * barSpriteRend.bounds.size.x, 0, 0);
        }
        else
        {
            return new Vector3(0, (reverse ? -0.5f : 0.5f) * barSpriteRend.bounds.size.y, 0);
        }
    }

    public void SetProgress(float value)
    {
        progress = Mathf.Clamp01(value);
        scaleTransform.localScale = new Vector3(Mathf.Lerp(minScale, maxScale, progress), scaleTransform.localScale.y);

        if (progress == 1f)
        {
            barSpriteRend.color = fullColor;
        }
        else
        {
            barSpriteRend.color = notFullColor;
        }
    }
}
