using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingGameObject : BaseGameObject {

    [SerializeField]
    protected float _scrollSpeed;
    protected Rigidbody2D _rigidbody;

    protected override void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        base.Start();
    }

    protected virtual void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + (Vector2.left * _scrollSpeed * Time.fixedDeltaTime));
    }
}
