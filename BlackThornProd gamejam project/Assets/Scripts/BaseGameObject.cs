using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameObject : MonoBehaviour {

    protected GameManager _gameManager;
    protected PixelGridSnapper _pixelGridSnapper;

    [SerializeField]
    protected Transform _childTransformToSnapToGrid;

    // Use this for initialization
    protected virtual void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _pixelGridSnapper = _gameManager.PixelGridSnapper;

        _pixelGridSnapper.SnapToTexelGrid(_childTransformToSnapToGrid, transform);
    }

    protected virtual void LateUpdate()
    {
        _pixelGridSnapper.SnapToScreenPixelGrid(_childTransformToSnapToGrid, transform);
    }
    

}
