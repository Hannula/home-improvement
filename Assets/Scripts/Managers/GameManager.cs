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
    public EventManager eventManager;
    public FadeToColor fade;
    public string sceneToLoad;
    public float gameSpeedModifier = 1f;
    public int regionNum = 1;
    public int money;
    public int score;

    // Testing
    public bool a;
    public bool b;
    public bool c;
    public bool d;
    public bool e;
    public bool f;
    public bool g;
    public bool h;
    public bool i;

    public Home home;
    public HomeData PlayerHome;
    public HomeData EnemyHome;
    public List<HomeUpgrade> Inventory;
    public BattleSkillHandler battleSkillHandler;
    public bool gameRunning;

    private bool gameJustStarted = true;

    public GameState State { get; private set; }

    public SceneTransition Transition { get; private set; }

    public bool GamePaused { get; private set; }

    public bool ActiveGame
    {
        get
        {
            return !GamePaused && Transition == SceneTransition.InScene;
        }
    }
    
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

    public float DeltaTimeVariable
    {
        get
        {
            if (GamePaused)
            {
                return 0f;
            }
            else
            {
                return Time.deltaTime * gameSpeedModifier;
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

    private void Init()
    {
        PlayerHome = HomeData.GenerateRandom(1, 2, 15);
        EnemyHome = HomeData.GenerateRandom(2, 2, 15);
        State = GameState.MainMenu;
        Transition = SceneTransition.InScene;
        InitScene();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitScene()
    {
        ui = FindObjectOfType<UIManager>();
        fade = FindObjectOfType<FadeToColor>();
        home = FindObjectOfType<Home>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Transition != SceneTransition.InScene)
        {
            UpdateSceneTransition();
        }
        else
        {
            // Testing
            if (a)
            {
                SFXPlayer.Instance.Play(Sound.Explosion1);
                a = false;
            }
            if (b)
            {
                SFXPlayer.Instance.Play(Sound.Explosion2);
                b = false;
            }
            if (c)
            {
                SFXPlayer.Instance.Play(Sound.Explosion3);
                c = false;
            }
            if (d)
            {
                SFXPlayer.Instance.Play(Sound.Explosion4);
                d = false;
            }
            if (e)
            {
                SFXPlayer.Instance.Play(Sound.Fireball1);
                e = false;
            }
            if (f)
            {
                SFXPlayer.Instance.Play(Sound.Fireball2);
                f = false;
            }
            if (g)
            {
                SFXPlayer.Instance.Play(Sound.Water);
                g = false;
            }
            if (h)
            {
                SFXPlayer.Instance.Play(Sound.Splat1);
                h = false;
            }
            if (i)
            {
                SFXPlayer.Instance.Play(Sound.Splat2);
                i = false;
            }
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
        gameRunning = false;
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
        if (Transition == SceneTransition.InScene)
        {
            State = GameState.Map;
            LoadScene(GameState.Map);
        }
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
        fade.StartFadeIn(false);

        SFXPlayer.Instance.InitAudioSrcPool();

        switch (State)
        {
            case GameState.MainMenu:
            {
                //ui.mainMenu.Activate(true);
                //MusicPlayer.Instance.Play(0, true);
                break;
            }
            case GameState.Map:
            {
                if (!gameRunning)
                {
                    StartNewGame();
                }

                eventManager = new GameObject("EventManager").AddComponent<EventManager>();
                MusicPlayer.Instance.Play(0, true);
                break;
            }
            case GameState.Battle:
            {
                StartBattle();
                break;
            }
        }
    }

    #endregion SceneManagement

    private void StartNewGame()
    {
        Debug.Log("New game started");
        regionNum = 1;

        PlayerHome = HomeData.GenerateRandom(2,3,50);
        home.Init(PlayerHome);

        Inventory = new List<HomeUpgrade>(20);
        for(int i = 0; i < 20; i++)
        {
            Inventory.Add(null);
        }

        gameRunning = true;
    }

    private void StartBattle()
    {
        battleSkillHandler = FindObjectOfType<BattleSkillHandler>();
        ui.ActivateBattleSkills(true);
        battleSkillHandler.Init();
        EnemyHome = HomeData.GenerateRandom(
            regionNum,
            Random.Range(2, regionNum + 2),
            Mathf.Min(60, 20 + 5 * regionNum));
        MusicPlayer.Instance.Play(1, true);
    }

    public void ReturnToMainMenu()
    {
        State = GameState.MainMenu;
        LoadScene(GameState.MainMenu);
    }

    public void NextRegion()
    {
        regionNum++;
        Debug.Log("Now entering region " + regionNum);
        saveData = new SaveDataPackage();
        LoadMapScene();
    }

    public void EndGame(bool win)
    {
        gameRunning = false;
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

    #region savingMap

    private SaveDataPackage saveData = new SaveDataPackage();
    public void SaveMapState(SaveDataPackage saveData)
    {
        this.saveData = saveData;
    }

    public SaveDataPackage LoadMapState()
    {
        return saveData;
    }

    #endregion
}
