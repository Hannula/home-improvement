using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIScreen
{
    public Button startButton;
    public Button quitButton;

    //public List<Text> loreTexts;
    //public Text loreText;
    //public List<string> lore;
    //public Vector2 bottomLeftCorner;
    //public Vector2 topRightCorner;
    //public float loreTextSpeed = 2f;
    //public Color backgroundColor;

    //private Vector2 targetLoreTextPoint;
    //private Color loreTextBasicColor;

    //private float colorChangeDist = 50f;
    //private float disappearDist = 5f;

    //// Start is called before the first frame update
    //private void Start()
    //{
    //    //bottomLeftCorner = new Vector2(loreText.transform.position.x - 600, loreText.transform.position.y - 600);
    //    //topRightCorner = new Vector2(loreText.transform.position.x + 600, loreText.transform.position.y + 600);
    //    targetLoreTextPoint = GetRandomPoint();
    //    loreTextBasicColor = loreText.color;
    //}

    // Update is called once per frame
    //private void Update()
    //{
    //    UpdateLoreText();
    //}

    //private void UpdateLoreText()
    //{
    //    if (loreText != null)
    //    {
    //        float dist = Vector2.Distance(loreText.transform.position, targetLoreTextPoint);
    //        Debug.Log(dist);
    //        if (dist > disappearDist)
    //        {
    //            Vector2 newPosition = loreText.transform.position;
    //            newPosition += (targetLoreTextPoint - (Vector2) loreText.transform.position).normalized * loreTextSpeed * Time.deltaTime;
    //            loreText.transform.position = newPosition;

    //            if (dist < colorChangeDist)
    //            {
    //                float ratio = dist - disappearDist / colorChangeDist;
    //                loreText.color = Color.Lerp(loreTextBasicColor, backgroundColor, 1 - ratio);
    //            }
    //        }
    //        else
    //        {
    //            targetLoreTextPoint = GetRandomPoint();
    //            loreText.color = loreTextBasicColor;
    //        }
    //    }
    //}

    //private Vector2 GetRandomPoint()
    //{
    //    Vector2 point = Vector2.zero;
    //    point.x = Random.Range(bottomLeftCorner.x, topRightCorner.x);
    //    point.y = Random.Range(bottomLeftCorner.y, topRightCorner.y);
    //    return point;
    //}

    public void StartGame()
    {
        GameManager.Instance.LoadNewGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
