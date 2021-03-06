﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public List<Sprite> images;
    public float imageDuration = 0.1f;
    public float timeToDisappear = 1f;
    public bool stopWhenGameOnPause = true;

    private SpriteRenderer sr;
    private PlaySoundAtSceneStart playSound;
    private float elapsedImageTime;
    private float elapsedDisappearTime;
    private int currentImage;

    // Start is called before the first frame update
    private void Start()
    {
        currentImage = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = images[currentImage];
        playSound = GetComponent<PlaySoundAtSceneStart>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.Instance.GamePaused || !stopWhenGameOnPause)
        {
            if (elapsedDisappearTime < timeToDisappear)
            {
                UpdateImage();

                elapsedDisappearTime += Time.deltaTime;
                    //(stopWhenGameOnPause ? GameManager.Instance.DeltaTime : Time.deltaTime);
                if (elapsedDisappearTime >= timeToDisappear)
                {
                    Disappear();
                }
            }
        }
    }

    private void UpdateImage()
    {
        elapsedImageTime += Time.deltaTime;
        if (elapsedImageTime >= imageDuration)
        {
            elapsedImageTime = 0f;
            currentImage++;
            if (currentImage >= images.Count)
            {
                currentImage = 0;
            }
            sr.sprite = images[currentImage];
        }
    }

    public void Disappear()
    {
        GameManager.Instance.ui.mainMenu.Activate(true);

        if (playSound != null)
        {
            AudioSource audioSrc = playSound.GetAudioSource();
            if (audioSrc != null)
            {
                audioSrc.Stop();
            }
        }

        MusicPlayer.Instance.Play(0, true);
        Destroy(gameObject);
    }
}
