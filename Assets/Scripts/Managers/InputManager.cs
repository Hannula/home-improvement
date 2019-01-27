using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        
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

        DebugInput();
    }

    private void MainMenuInput()
    {
        
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
