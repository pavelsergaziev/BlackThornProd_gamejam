using UnityEngine;
using System.Collections;
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
    /// Скорость стрельбы(секнды до следующего выстрела)
    /// </summary>
    [Header("Параметры оружия")]
    [Tooltip("Скорость стрельбы(секнды до следующего выстрела)")]
    [SerializeField]
    private float _timeBetweenShots;
    /// <summary>
    /// Скорость снаряда
    /// </summary>
    [Tooltip("Скорость снаряда")]
    [SerializeField]
    private float _rocketSpeed;

    [SerializeField]
    private int _maxRocketCount;
    [SerializeField]
    private float _secToGetRocket;
    [SerializeField]
    private int _codeStrokesToCollect;
    [SerializeField]
    private int _startRocketCount;

    private Animator _animator;
    public float ChangeScrollSpeed;
    
    public float ScrollTime;
    public ScrollingGameObject DeadLine;


    /// <summary>
    /// Ссылка на префаб гарпуна
    /// </summary>
    [Tooltip("Ссылка на префаб гарпуна")]
    [SerializeField]
    private Harpoon _harpoon;
    /// <summary>
    /// Скорость стрельбы гарпуна
    /// </summary>
    [Tooltip("Скорость стрельбы гарпуна")]
    [SerializeField]
    private float _harpoonShootSpeed;
    /// <summary>
    /// Дальность стрельбы гарпуном
    /// </summary>
    [Tooltip("Дальность стрельбы гарпуном")]
    [SerializeField]
    private float _harpoonMaxShootDistance;
    /// <summary>
    /// Скорость возвращения гарпуна
    /// </summary>
    [Tooltip("Скорость возвращения гарпуна")]
    [SerializeField]
    private float _harpoonBackSpeed;

    /// <summary>
    /// Ссылка на обьект для проверки, находится ли игрок на земле
    /// </summary>
    [Header("Ссылки на обьекты")]
    [Tooltip("Ссылка на Transform для проверки, находится ли игрок на земле")]
    [SerializeField]
    private Transform _groundChek;
    /// <summary>
    /// Ссылка на Transform оружия
    /// </summary>
    [Tooltip("Ссылка на Transform оружия")]
    [SerializeField]
    private Transform _weaponTransform;
    /// <summary>
    /// Ссылка на Transform места создания пули
    /// </summary>
    [Tooltip("В этом месте создается пуля")]
    [SerializeField]
    public Transform ShootingPoint;
    /// <summary>
    /// Ссылка на префаб пули
    /// </summary>
    [Tooltip("Префаб пули типа Bullet")]
    [SerializeField]
    private Bullet _rocket;
    #endregion

    /// <summary>
    /// Ссылка на RigidBody
    /// </summary>
    private Rigidbody2D _rigidBody;
    /// <summary>
    /// Можно ли управлять игроком?
    /// </summary>
    public bool IsControllable { get; private set; }
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
    /// <summary>
    /// Ссылка на главную игровую камеру
    /// </summary>
    private Camera _mainCamera;
    /// <summary>
    /// Спрятано ли оружие?
    /// </summary>
    public bool WeaponIsHide { get; private set; }
    /// <summary>
    /// Можно стрелять?
    /// </summary>
    private bool _canFire;
    /// <summary>
    /// Время после последнего выстрела
    /// </summary>
    private float _timeAfterLastShot = 0;
    /// <summary>
    /// Можно стрелять гарпуном?
    /// </summary>
    [HideInInspector]
    public bool CanHarpoon = true;

    [HideInInspector]
    public int CurrentRocketCount;
    private float _timeAfterGetRocket = 0;
    private UiManager _uiManager;
    private int _codeStrokes = 0;
    private SoundOnObject _soundController;
    private float _normalDeadLineSpeed;
    private float _normalPlayerScrollSpeed;

    

    protected override void Start()
    {
        base.Start();
        _rigidBody = GetComponent<Rigidbody2D>();
        _mainCamera = FindObjectOfType<Camera>();
        _uiManager = FindObjectOfType<UiManager>();
        
        CurrentRocketCount = _startRocketCount;
        _maxRocketCount = CurrentRocketCount;
        _soundController = GetComponent<SoundOnObject>();
        IsControllable = true;// Временно для теста. Должен включаться после кат сцены
        WeaponIsHide = true;// Временно для теста. Должен включаться после кат сцены
        _canFire = true;// Временно для теста. Должен включаться после кат сцены
        _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
        _normalDeadLineSpeed = DeadLine.ScrollSpeed;
        _normalPlayerScrollSpeed = ScrollSpeed;
        _animator = _childTransformToSnapToGrid.GetComponent<Animator>();
    }
    private void Update()
    {
        if (IsControllable)
        {
            ChekCanJump();
            Scroll(_normalPlayerScrollSpeed);
            RotateWeapon();
            Shoot();
            SetRocketsCount();
        }

        _animator.SetBool("isGrounded", IsGrounded);
    }
    private void FixedUpdate()
    {
        if (CanJump)
        {
            CanJump = false;
            _rigidBody.AddForce(Vector2.up * JumpForce * Time.deltaTime);
            var rnd = Random.Range(1, 6);
            _soundController.PlaySound("JumpFx_" + rnd.ToString(), false);
        }
        foreach (Transform child in _groundChek)
        {
            if (Physics2D.Linecast(transform.position, child.position, _groundLayerMask))
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
    /// <summary>
    /// Проверяет, можно ли прыгать и устанавливает соответствующее значение переменной CanJump
    /// </summary>
    private void ChekCanJump()
    {
        var jump = Input.GetAxisRaw("Jump") > 0 && IsGrounded && Mathf.Abs(_rigidBody.velocity.y) < 0.01;
        if (jump)
        {
            CanJump = true;
        }
        else
        {
            CanJump = false;
        }
    }
    /// <summary>
    /// Отвечает за перемещение игрока по оси X
    /// </summary>
    /// <param name="scrollSpeed">Скорость перемещения по оси X</param>
    private void Scroll(float scrollSpeed)
    {
        _rigidBody.transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
    }
    /// <summary>
    /// Отвечает за вращение оружия. Оружие следит за курсором мыши
    /// </summary>
    private void RotateWeapon()
    {
        if (WeaponIsHide != false)
        {
            Vector3 difference = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - _weaponTransform.position;
            float rotZ = Mathf.Clamp(Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg, -80, 80);
            _weaponTransform.rotation = Quaternion.Euler(0, 0, rotZ);
        }
    }
    /// <summary>
    /// Отвечает за стрельбу 
    /// </summary>
    private void Shoot()
    {
        if (_canFire )
        {

            if (Input.GetMouseButtonDown(0) )
            {
                if (CurrentRocketCount > 0)
                {
                    var tmpBullet = Instantiate(_rocket, ShootingPoint.position, _weaponTransform.rotation);
                    tmpBullet.Speed = _rocketSpeed;
                    _canFire = false;
                    _timeAfterLastShot = 0;

                    CurrentRocketCount -= 1;
                }
                else
                {
                    _soundController.PlaySound("NoAmmoFx", false);
                }


            }
            
            if (Input.GetMouseButtonDown(1))
            {
                if (CanHarpoon)
                {
                    CanHarpoon = false;
                    var tmpHarpoon = Instantiate(_harpoon, ShootingPoint.position, _weaponTransform.rotation);
                    tmpHarpoon.ShootSpeed = _harpoonShootSpeed;
                    tmpHarpoon.MaxShootDistance = _harpoonMaxShootDistance;
                    tmpHarpoon.BackSpeed = _harpoonBackSpeed;
                    _canFire = false;
                    _timeAfterLastShot = 0;
                }
                else
                {
                    _soundController.PlaySound("NoAmmoFx", false);
                }

            }
        }
        else
        {
            _timeAfterLastShot += Time.deltaTime;
            if (_timeAfterLastShot>=_timeBetweenShots)
            {
                _canFire = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PickUp")
        {
            var collisionScript = collision.GetComponent<ScrollingGameObject>();
            collisionScript.SwitchVisibility();
            PickUp(collision.GetComponent<ScrollingGameObject>().TypeOf);
            
        }
    }
    /// <summary>
    /// Срабатывает когда игрок подбирает бафф или дебафф или что душе угодно
    /// </summary>
    /// <param name="typeOf">Что подобрал</param>
    public void PickUp(TypeOfObject typeOf)
    {
        var rnd = Random.Range(1, 7);

        switch (typeOf)
        {
            
            case TypeOfObject.buff:
                SpeedUpPlayer();
                _soundController.PlaySound("GoodFx_" + rnd.ToString(), false);
                break;
            case TypeOfObject.debuff:
                SpeedupDeadLine();
                _soundController.PlaySound("BadFx_" + rnd.ToString(), false);
                break;
            case TypeOfObject.bug:
                SpeedupDeadLine();
                _soundController.PlaySound("BadFx_" + rnd.ToString(), false);
                break;
            case TypeOfObject.codeStroke:
                _soundController.PlaySound("GoodFx_" + rnd.ToString(), false);
                SetCodeStrokesCount();
                
                break;
            default:
                break;
        }
    }
    private void SetRocketsCount()
    {
        if (CurrentRocketCount < _maxRocketCount)
        {
            if (_timeAfterGetRocket<=0)
            {
                CurrentRocketCount += 1;
                _timeAfterGetRocket = _secToGetRocket;
            }
            else
            {
                _timeAfterGetRocket -= Time.deltaTime;
            }
        }
        _uiManager.UpdateRocketsText(CurrentRocketCount.ToString());
    }
    private void SetCodeStrokesCount()
    {
        if (_codeStrokes<_codeStrokesToCollect)
        {
            _codeStrokes += 1;
        }
        else
        {
            Debug.Log("GGWP");
        }
        _uiManager.UpdateCodeStrokesText((_codeStrokesToCollect - _codeStrokes).ToString());
    }
    public void SlowDownplayer()
    {
        StartCoroutine(ChagePlayerScrollSpeed(ScrollTime, -ChangeScrollSpeed));
    }
    public void SpeedUpPlayer()
    {
        StartCoroutine(ChagePlayerScrollSpeed(ScrollTime, ChangeScrollSpeed));
    }
    IEnumerator  ChagePlayerScrollSpeed(float time, float speed)
    {
        _normalPlayerScrollSpeed = ScrollSpeed;
        _normalPlayerScrollSpeed = speed;
        yield return new WaitForSeconds(time);
        _normalPlayerScrollSpeed = ScrollSpeed;
    }
    IEnumerator ChangeDedLineScrollSpeed(float time, float speed)
    {

        DeadLine.ScrollSpeed = _normalDeadLineSpeed;
        DeadLine.ScrollSpeed += speed;
        yield return new WaitForSeconds(time);
        DeadLine.ScrollSpeed = _normalDeadLineSpeed;
    }
    public void SlowDownDedLine()
    {
        StartCoroutine(ChangeDedLineScrollSpeed(ScrollTime, ChangeScrollSpeed));
    }
    public void SpeedupDeadLine()
    {
        StartCoroutine(ChangeDedLineScrollSpeed(ScrollTime, -ChangeScrollSpeed));
    }

}
