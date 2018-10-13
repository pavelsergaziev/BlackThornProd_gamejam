using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseGameObject
{
    //ВРЕМЕННЫЙ ТЕСТОВЫЙ ПЛЕЕРКОНТРОЛЛЕР

    [SerializeField]
    private int _movemementSpeed;
    [SerializeField]
    private int _jumpForce;

    private Rigidbody2D _rigidBody;

    [SerializeField]
    private Transform _groundCheck;

    private int _groundLayerMask;

    private float _horizontalAxisValue;

    private bool _grounded;
    private bool _doJumpInFixedUpdate;
    

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        _rigidBody = GetComponent<Rigidbody2D>();

        _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {

        //простенькая перемещалка
        _horizontalAxisValue = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Jump") && _grounded)
            _doJumpInFixedUpdate = true;

    }

    void FixedUpdate()
    {
        _rigidBody.AddForce(new Vector2(_horizontalAxisValue * _movemementSpeed, 0));

        if (_doJumpInFixedUpdate)
        {
            _doJumpInFixedUpdate = false;
            _rigidBody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }



        _grounded = false;

        foreach (Transform child in _groundCheck)
            if (Physics2D.Linecast(transform.position, child.position, _groundLayerMask))
            {
                _grounded = true;
                break;
            }
    }
}
