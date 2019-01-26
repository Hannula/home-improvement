﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float progress = 1f;

    public float minScale;
    public float maxScale;
    public bool horizontal = true;
    public bool reverse;
    public GameObject bar;

    private SpriteRenderer barSpriteRend;
    private Vector3 defaultScale;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        barSpriteRend = bar.GetComponent<SpriteRenderer>();
        defaultScale = bar.transform.localScale;

        startPosition = bar.transform.position;
        if (horizontal)
        {
            startPosition.x += (reverse? 0.5f : -0.5f) * barSpriteRend.bounds.size.x;
        }
        else
        {
            startPosition.y += (reverse ? 0.5f : -0.5f) * barSpriteRend.bounds.size.y;
        }

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
        if (horizontal)
        {
            Vector3 newScale = new Vector3(Mathf.Lerp(minScale, maxScale, progress), defaultScale.y, defaultScale.z);
            bar.transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = new Vector3(defaultScale.x, Mathf.Lerp(minScale, maxScale, progress), defaultScale.z);
            bar.transform.localScale = newScale;
        }

        bar.transform.position = startPosition + GetDifferenceFromStartToCurrPos();
    }
}
