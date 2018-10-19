using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject _player;

    [SerializeField]
    private GameObject _deadline;
    

    private Camera _camera;

    private bool _gameHasBeenPlayedAlready;

    
    

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

        _camera = FindObjectOfType<Camera>();
        _soundManager = FindObjectOfType<SoundManager>();
        _cursorController = FindObjectOfType<CursorController>();
        _cutscenePlayer = FindObjectOfType<Cutscene>();
        _uiManager = FindObjectOfType<UiManager>();
        _levelGenerationController = FindObjectOfType<LevelGenerationController>();

        _player = FindObjectOfType<PlayerController>().gameObject;

        _camera.orthographicSize = (float)Screen.height / _texelsPerUnit / 2 / _pixelsPerTexel;

        _gameHasBeenPlayedAlready = true;

        Invoke("DeactivateObjectsBeforeStart", 0.1f);
    }


//запустить бесконечную корутину со скоростью, зависящей от средней скорости перемещения уровня, и в ней запускать генерацию уровня.



    private void DeactivateObjectsBeforeStart()
    {
        _player.SetActive(false);
        _deadline.SetActive(false);
    }

    public void StartNewGame()
    {

        //_soundManager.StartNewGameWhithCutScene();        

        //и поставить условие в зависимости от _gameHasBeenPlayedAlready
        PlayCutscene();
    }

    private void PlayCutscene()
    {
       _soundManager.StartNewGameWhithCutScene(); 

        _cutscenePlayer.StartCutscene();
        _uiManager.StartGame();
        _cursorController.SwitchToCustomCursor();



        //_levelGenerationController.StartGeneration();
    }

    public void StartGameplay()
    {
        _levelGenerationController.StartGeneration();
        _player.SetActive(true);
        _uiManager.InGame = true;
        _deadline.SetActive(true);
    }
}
