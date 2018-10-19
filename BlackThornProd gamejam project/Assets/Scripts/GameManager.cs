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

        _camera.orthographicSize = (float)Screen.height / _texelsPerUnit / 2 / _pixelsPerTexel;

        _gameHasBeenPlayedAlready = true;
    }


//запустить бесконечную корутину со скоростью, зависящей от средней скорости перемещения уровня, и в ней запускать генерацию уровня.


    public void StartNewGame()
    {
        _soundManager.StartNewGameWhithCutScene();        
        _cutscenePlayer.StartCutscene();
        _uiManager.StartGame();
        _cursorController.SwitchToCustomCursor();
    }
}
