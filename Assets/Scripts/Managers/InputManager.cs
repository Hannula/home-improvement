using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    IntroScreen intro;

    // Start is called before the first frame update
    private void Start()
    {
        intro = FindObjectOfType<IntroScreen>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.MainMenu)
        {
            MainMenuInput();
        }
        else
        {
            GameInput();
        }

#if UNITY_EDITOR
        DebugInput();
#endif
    }

    private void MainMenuInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (intro != null)
            {
                intro.Disappear();
            }

            if (GameManager.Instance.ui.mainMenu.instructionsScreen.activeSelf)
            {
                GameManager.Instance.ui.mainMenu.CloseInstructions();
            }
        }
        else if (Input.GetKey(KeyCode.Space) ||
                 Input.GetKey(KeyCode.Return) ||
                 Input.GetKey(KeyCode.Escape))
        {
            if (intro != null)
            {
                intro.Disappear();
            }
        }
    }

    private void GameInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) &&
            !GameManager.Instance.eventManager.eventActive)
        {
            GameManager.Instance.PauseGame(!GameManager.Instance.GamePaused);
        }
    }

    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            GameManager.Instance.eventManager.EndEventWithoutResults();
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            GameManager.Instance.EndGame(false);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            GameManager.Instance.LoadMapScene();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Break();
        }
    }
}
