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
    private int _pixelsPerUnit = 32;
    [SerializeField]
    private int _pixelsPerTexel = 3;

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

        _pixelGridSnapper = new PixelGridSnapper(_pixelsPerUnit, _pixelsPerTexel);


    }

    void Start()
    {

        _camera = FindObjectOfType<Camera>();

        _camera.orthographicSize = (float)Screen.height / _pixelsPerUnit / 2 / _pixelsPerTexel;

    }
}
