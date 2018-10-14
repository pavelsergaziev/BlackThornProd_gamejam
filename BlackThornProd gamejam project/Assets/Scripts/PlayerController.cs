using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Логика и поля, отвечающие за игрока
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : FreeMovingGameObject
{
    #region Поля для инициализации в инспекторе
    /// <summary>
    /// Сила прыжка
    /// </summary>
    [Header("Параметры движения игрока")]
    [Tooltip("Сила прыжка")]
    public float JumpForce;
    /// <summary>
    /// Скорость скролла игрока
    /// </summary>
    [Tooltip("Скорость скролла игрока")]
    public float ScrollSpeed;
    /// <summary>
    /// Ссылка на обьект для проверки, находится ли игрок на земле
    /// </summary>
    [Space(10f)]
    [Header("Ссылки на обьекты")]
    [Tooltip("Ссылка на Transform для проверки, находится ли игрок на земле")]
    [SerializeField]
    private Transform _groundChek;
    #endregion

    /// <summary>
    /// Ссылка на RigidBody
    /// </summary>
    private Rigidbody2D _rigidBody;
    /// <summary>
    /// Можно ли управлять игроком?
    /// </summary>
    public bool IsControllable { get;private set; }
    /// <summary>
    /// Игрок находится на земеле?
    /// </summary>
    public bool IsGrounded { get; private set; }
    /// <summary>
    /// Игрок может прыгать?
    /// </summary>
    public bool CanJump { get; private set; }
    /// <summary>
    /// Маска слоя земли
    /// </summary>
    private int _groundLayerMask;
    



    protected override void Start()
    {
        base.Start();
        _rigidBody = GetComponent<Rigidbody2D>();

        IsControllable = true;// Временно для теста. Должен включаться после кат сцены
        if (_groundChek == null)
        {
            throw new System.Exception("Cant find GroundChek transform on PlayersController obj named "+ name );
        }
        _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");


    }
    private void Update()
    {
        if (IsControllable)
        {
            var jump = Input.GetAxisRaw("Jump") > 0 && IsGrounded&& Mathf.Abs( _rigidBody.velocity.y)<0.01;
            if (jump)
            {
                CanJump = true;
                
            }
        }
        _rigidBody.transform.position +=Vector3.right * ScrollSpeed*Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (CanJump)
        {
            CanJump = false;
            _rigidBody.AddForce(Vector2.up * JumpForce * Time.deltaTime);
        }
        foreach (Transform child in _groundChek)
        {
            if (Physics2D.Linecast(transform.position,child.position,_groundLayerMask))
            {
                IsGrounded = true;
                break;
            }
            else
            {
                IsGrounded = false;
            }
        }
        
    }
}
