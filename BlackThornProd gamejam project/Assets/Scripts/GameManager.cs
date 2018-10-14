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

    [Header("Настройки для генерации уровня")]
    [SerializeField]
    private LevelLayoutParameters _levelLayoutParameters = new LevelLayoutParameters();

    private PixelGridSnapper _pixelGridSnapper;
    public PixelGridSnapper PixelGridSnapper { get { return _pixelGridSnapper; } }
    

    private Camera _camera;
    

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

        _camera.orthographicSize = (float)Screen.height / _texelsPerUnit / 2 / _pixelsPerTexel;
        

    }

//запустить бесконечную корутину со скоростью, зависящей от средней скорости перемещения уровня, и в ней запускать генерацию уровня.


}
