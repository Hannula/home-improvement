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

    private void CloseMainMenuScreens()
    {
        if (intro != null)
        {
            intro.Disappear();
        }

        if (GameManager.Instance.ui.mainMenu.instructionsScreen.activeSelf)
        {
            GameManager.Instance.ui.mainMenu.CloseInstructions();
        }
        else if (GameManager.Instance.ui.mainMenu.creditsScreen.activeSelf)
        {
            GameManager.Instance.ui.mainMenu.CloseCredits();
        }
    }

    private void MainMenuInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CloseMainMenuScreens();
        }
        else if (Input.GetKey(KeyCode.Space) ||
                 Input.GetKey(KeyCode.Return) ||
                 Input.GetKey(KeyCode.Escape))
        {
            CloseMainMenuScreens();
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
        // Skip event
        if (Input.GetKeyDown(KeyCode.Home))
        {
            GameManager.Instance.eventManager.EndEventWithoutResults();
        }
        // Lose game
        else if (Input.GetKeyDown(KeyCode.End))
        {
            GameManager.Instance.EndGame(false);
        }
        // Go to map
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            GameManager.Instance.LoadMapScene();
        }
        // Win battle
        else if (GameManager.Instance.State == GameManager.GameState.Battle
                 && Input.GetKeyDown(KeyCode.Keypad1))
        {
            GameManager.Instance.BattleStatus = GameManager.BattleState.Won;
            GameManager.Instance.EndBattle();
            GameManager.Instance.LoadMapScene();
        }
        // Pause the game in editor
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Break();
        }
    }
}
