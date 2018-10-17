using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingGameObject : BaseGameObject {
    /// <summary>
    /// Тип обьекта для взаимодействия
    /// </summary>
    public TypeOfObject TypeOf;





    [SerializeField]
    protected float _scrollSpeed;
    public float ScrollSpeed { get { return _scrollSpeed; } set { _scrollSpeed = value; } }
    
    protected Rigidbody2D _rigidbody;

    //protected float _movementIncrement;
    //protected float _currentVirtualScrolledDistance;
    //protected float _currentSnappedScrolledDistance;
    //protected float _currentVirtualScrolledExcess;

    private bool _isPhysical;


    protected override void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody != null)
            _isPhysical = true;

        base.Start();

        //_movementIncrement = 1 / (float)(_gameManager.TexelsPerUnit * _gameManager.PixelsPerTexel);
    }

    protected virtual void FixedUpdate()
    {
        if (!_isPhysical)
            return;

        _rigidbody.MovePosition(_rigidbody.position + (Vector2.left * _scrollSpeed * Time.fixedDeltaTime));
    }

    protected virtual void Update()
    {
        if (_isPhysical)
            return;

        transform.position = new Vector3(transform.position.x - (_scrollSpeed * Time.deltaTime), transform.position.y, transform.position.z);
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
