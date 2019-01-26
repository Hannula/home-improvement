using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Statics
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion Statics

    public enum GameState
    {
        MainMenu = 0,
        Map = 1,
        Battle = 2
    }

    public enum SceneTransition
    {
        InScene = 0,
        ExitingScene = 1,
        EnteringScene = 2
    }

    public UIManager ui;
    public FadeToColor fade;
    public string sceneToLoad;
    public int score;

    public HomeData PlayerHome;

    private bool gameJustStarted = true;

    public GameState State { get; private set; }

    public SceneTransition Transition { get; private set; }

    public bool GamePaused { get; private set; }

    public float DeltaTime
    {
        get
        {
            if (GamePaused)
            {
                return 0f;
            }
            else
            {
                return Time.deltaTime;
            }
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Start()
    {
        PlayerHome = new HomeData(new FloorData(5), new FloorData(5));
    }

    private void Init()
    {
        State = GameState.MainMenu;
        Transition = SceneTransition.InScene;
        InitScene();
        ui.mainMenu.Activate(true);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitScene()
    {
        ui = FindObjectOfType<UIManager>();
        fade = FindObjectOfType<FadeToColor>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Transition != SceneTransition.InScene)
        {
            UpdateSceneTransition();
        }
    }

    #region SceneManagement

    private void StartLoadingScene()
    {
        Transition = SceneTransition.ExitingScene;
        fade.StartFadeOut(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
            {
                sceneToLoad = "MainMenu";
                break;
            }
            case GameState.Map:
            {
                sceneToLoad = "MapScene";
                break;
            }
            case GameState.Battle:
            {
                sceneToLoad = "BattleScene";
                break;
            }
        }

        if (sceneToLoad != null && sceneToLoad.Length > 0)
        {
            StartLoadingScene();
        }
    }

    public void LoadNewGame()
    {
        State = GameState.Map;
        LoadScene(GameState.Map);
    }

    public void LoadBattleScene()
    {
        State = GameState.Battle;
        LoadScene(GameState.Battle);
    }

    public void LoadMapScene()
    {
        State = GameState.Map;
        LoadScene(GameState.Map);
    }

    private void UpdateSceneTransition()
    {
        if (Transition == SceneTransition.ExitingScene && fade.FadedOut)
        {
            LoadScene(sceneToLoad);
            sceneToLoad = "";
        }
        else if (Transition == SceneTransition.EnteringScene && fade.FadedIn)
        {
            Transition = SceneTransition.InScene;
            PauseGame(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameJustStarted)
        {
            gameJustStarted = false;
            return;
        }

        Transition = SceneTransition.EnteringScene;
        InitScene();
        //ui.CloseMenus();
        fade.StartFadeIn(false);

        switch (State)
        {
            case GameState.MainMenu:
            {
                ui.mainMenu.Activate(true);
                break;
            }
        }
    }

    #endregion SceneManagement

    private void StartNewGame()
    {
        // TODO: Add new game starting stuff here.
        ReturnToMapScene();
    }

    private void StartBattle()
    {
        // TODO: Add battle starting stuff here.
    }

    private void ReturnToMapScene()
    {
        // TODO: Add map scene starting stuff here.
    }

    public void ReturnToMainMenu()
    {
        State = GameState.MainMenu;
        LoadScene(GameState.MainMenu);
    }

    public void EndGame(bool win)
    {
        ui.EndGame(win);
    }

    public void PauseGame(bool pause)
    {
        if (GamePaused != pause)
        {
            GamePaused = pause;
            if (GamePaused)
            {
                ui.pauseMenu.Activate(true);
            }
            else
            {
                ui.pauseMenu.Activate(false);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
