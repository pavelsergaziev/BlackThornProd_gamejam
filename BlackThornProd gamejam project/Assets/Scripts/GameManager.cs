using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

/// <summary>
/// Гейм-менеджер, синглтон
/// </summary>

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    [Header("Настройки пикселей")]
    [SerializeField]
    private int _texelsPerUnit = 32;
    public int TexelsPerUnit { get { return _texelsPerUnit; } }

    [SerializeField]
    private int _pixelsPerTexel = 3;
    public int PixelsPerTexel { get { return _pixelsPerTexel; } }
    

    private PixelGridSnapper _pixelGridSnapper;
    public PixelGridSnapper PixelGridSnapper { get { return _pixelGridSnapper; } }

    private SoundManager _soundManager;
    private UiManager _uiManager;
    private CursorController _cursorController;
    private Cutscene _cutscenePlayer;
    private LevelGenerationController _levelGenerationController;
    private PlayerController _playerController;
        
    private GameObject _deadline;


    private Camera _camera;

    private bool _gameHasBeenPlayedAlready;

    private bool _shouldStartFromGameplay;

    private enum GameState { mainMenu, cutscene, gameplay, paused, dead, won }
    private GameState _currentGameState;
    

    void Awake()
    {
        #region создание и проверка синглтона
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        #endregion

        _pixelGridSnapper = new PixelGridSnapper(_texelsPerUnit, _pixelsPerTexel);

    }

    void Start()
    {
        //_gameHasBeenPlayedAlready = true;//ТЕСТОВОЕ, УБРАТЬ!!!

        Debug.Log("старт геймменеджера");
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("сцена загружена");
        Invoke("Initialize", 0.1f);
    }

    private void Initialize()
    {
        Debug.Log("инициализация геймменеджера");

        if (_pixelGridSnapper == null)
            _pixelGridSnapper = new PixelGridSnapper(_texelsPerUnit, _pixelsPerTexel);

        _camera = FindObjectOfType<Camera>();
        _soundManager = FindObjectOfType<SoundManager>();
        _cursorController = FindObjectOfType<CursorController>();
        _cutscenePlayer = FindObjectOfType<Cutscene>();
        _uiManager = FindObjectOfType<UiManager>();
        _levelGenerationController = FindObjectOfType<LevelGenerationController>();

        _playerController = FindObjectOfType<PlayerController>();
        _deadline = FindObjectOfType<DeadlineIdentifier>().gameObject;

        _camera.orthographicSize = (float)Screen.height / _texelsPerUnit / 2 / _pixelsPerTexel;
        

        _currentGameState = GameState.mainMenu;

        Debug.Log(_gameHasBeenPlayedAlready);

        if (_shouldStartFromGameplay)
        {
            _shouldStartFromGameplay = false;
            _soundManager.StartNewGameWhithoutCutScene();
            StartGameplay();
        }
        else
            Invoke("DeactivateObjectsBeforeStart", 0.1f);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            switch (_currentGameState)
            {
                case GameState.mainMenu:
                    break;
                case GameState.cutscene:
                    break;
                case GameState.gameplay: PauseGame();
                    break;
                case GameState.paused: ResumeGame();
                    break;
                case GameState.dead:
                    break;
                case GameState.won:
                    break;
                default:
                    break;
            }
        }
    }


    private void DeactivateObjectsBeforeStart()
    {

        Debug.Log("deactivation");

        _playerController.gameObject.SetActive(false);
        _playerController.IsControllable = false;

        _deadline.SetActive(false);
    }

    public void StartNewGame()
    {

        //_soundManager.StartNewGameWhithCutScene();        

        //и поставить условие в зависимости от _gameHasBeenPlayedAlready
        if (!_gameHasBeenPlayedAlready)
            PlayCutscene();
        else
        {            
            _soundManager.StartNewGameWhithoutCutScene();
            StartGameplay();
        }
    }

    private void PlayCutscene()
    {
        _currentGameState = GameState.cutscene;

       _soundManager.StartNewGameWhithCutScene();
        _cutscenePlayer.StartCutscene();
        _uiManager.StartGame();
        _cursorController.SwitchToCustomCursor();

        Debug.Log(_currentGameState);
    }

    public void StartGameplay()
    {
        _currentGameState = GameState.gameplay;

        _levelGenerationController.StartGeneration();
        _uiManager.StartGameplay();
        _playerController.gameObject.SetActive(true);
        _playerController.IsControllable = true;
        _deadline.SetActive(true);

        _gameHasBeenPlayedAlready = true;

        Debug.Log(_currentGameState);
    }

    public void ReturnToMainMenu()
    {
        _currentGameState = GameState.mainMenu;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        Debug.Log(_currentGameState);
    }

    public void PauseGame()
    {
        _currentGameState = GameState.paused;

        Time.timeScale = 0.01f;
        _soundManager.PauseMusic();
        _uiManager.ShowHidePouseMenu();
        _cursorController.SwitchToNormalCursor();
        _playerController.GetComponent<PlayerController>().IsControllable = false;

        Debug.Log(_currentGameState);
    }

    public void ResumeGame()
    {
        _currentGameState = GameState.gameplay;

        Time.timeScale = 1f;
        _soundManager.ResumeMusic();
        _uiManager.ShowHidePouseMenu();
        _cursorController.SwitchToCustomCursor();
        _playerController.GetComponent<PlayerController>().IsControllable = true;

        Debug.Log(_currentGameState);
    }

    public void Die()
    {
        _currentGameState = GameState.dead;

        Time.timeScale = 0f;
        _uiManager.ShowDeadMenu();
        _cursorController.SwitchToNormalCursor();

        Debug.Log(_currentGameState);
    }

    public void Win()
    {
        _currentGameState = GameState.won;

        Time.timeScale = 0;
        _playerController.IsControllable = false;
        _cursorController.SwitchToNormalCursor();
        _uiManager.ShowGGPannel();
    }

    public void RestartGameplay()
    {

        _shouldStartFromGameplay = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }



}
