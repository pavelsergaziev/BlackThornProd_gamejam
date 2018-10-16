using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameObject : MonoBehaviour {

    protected GameManager _gameManager;
    protected PixelGridSnapper _pixelGridSnapper;

    [SerializeField]
    protected Transform _childTransformToSnapToGrid;
    public Transform ChildTransformToSnapToGrid { get { return _childTransformToSnapToGrid; } }
    

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

    public virtual void SwitchVisibility()
    {
        _childTransformToSnapToGrid.gameObject.SetActive(!_childTransformToSnapToGrid.gameObject.activeSelf);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = !collider.enabled;
    }

}
