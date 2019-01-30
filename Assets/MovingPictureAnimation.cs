using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPictureAnimation : MonoBehaviour
{
    public List<Sprite> images;
    public float imageDuration = 0.1f;
    public GameManager.GameState playInGameState = GameManager.GameState.Battle;
    public bool playInAnyGameState;
    public bool stopWhenGameOnPause = true;

    private SpriteRenderer sr;
    private float elapsedImageTime;
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
        if (!GameManager.Instance.GamePaused || !stopWhenGameOnPause)
        {
            if (playInAnyGameState
                || GameManager.Instance.State == playInGameState)
            {
                UpdateImage();
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
}
