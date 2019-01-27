using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScreen : MonoBehaviour
{
    public List<Sprite> images;
    public float imageDuration = 0.1f;
    public float timeToDisappear = 1f;

    private SpriteRenderer sr;
    private float elapsedImageTime;
    private float elapsedDisappearTime;
    private int currentImage;

    // Start is called before the first frame update
    void Start()
    {
        currentImage = 0;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = images[currentImage];
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedDisappearTime < timeToDisappear)
        {
            UpdateImage();

            elapsedDisappearTime += Time.deltaTime;
            if (elapsedDisappearTime >= timeToDisappear)
            {
                Disappear();
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
        MusicPlayer.Instance.Play(0, true);
        Destroy(gameObject);
    }
}
