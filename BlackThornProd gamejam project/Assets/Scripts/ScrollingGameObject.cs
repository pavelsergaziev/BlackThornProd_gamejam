using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingGameObject : BaseGameObject {

    [SerializeField]
    protected float _scrollSpeed;

    protected Rigidbody2D _rigidbody;

    //protected float _movementIncrement;
    //protected float _currentVirtualScrolledDistance;
    //protected float _currentSnappedScrolledDistance;
    //protected float _currentVirtualScrolledExcess;
    


    protected override void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        base.Start();

        //_movementIncrement = 1 / (float)(_gameManager.TexelsPerUnit * _gameManager.PixelsPerTexel);
    }

    protected virtual void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + (Vector2.left * _scrollSpeed * Time.fixedDeltaTime));
    }

    
    //public void ResetMovement()
    //{
    //    _currentVirtualScrolledDistance = 0;
    //    _currentVirtualScrolledExcess = 0;
    //    _currentSnappedScrolledDistance = 0;
    //}



    //Попробовал иначе лепить к сетке - не помогает
    //
    //protected virtual void Update()
    //{
    //    _currentVirtualScrolledDistance += _scrollSpeed * Time.deltaTime;

    //    if (_currentVirtualScrolledDistance / _movementIncrement >= 1)
    //    {
    //        _currentVirtualScrolledExcess = _currentVirtualScrolledDistance % _movementIncrement;
    //        _currentSnappedScrolledDistance = _currentVirtualScrolledDistance - _currentVirtualScrolledExcess;
    //        _currentVirtualScrolledDistance = _currentVirtualScrolledExcess;
    //    }
    //}

    //protected virtual void FixedUpdate()
    //{

    //    if (_currentSnappedScrolledDistance != 0)
    //    {
    //        _rigidbody.MovePosition(new Vector2(_rigidbody.position.x - _currentSnappedScrolledDistance, _rigidbody.position.y));
    //        _currentSnappedScrolledDistance = 0;
    //    }

    //}
}
