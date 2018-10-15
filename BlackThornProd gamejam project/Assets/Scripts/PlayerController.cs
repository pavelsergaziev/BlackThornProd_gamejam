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
    private float _bulletSpeed;
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
    private Bullet _bullet;
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

    protected override void Start()
    {
        base.Start();
        _rigidBody = GetComponent<Rigidbody2D>();
        _mainCamera = FindObjectOfType<Camera>();
        IsControllable = true;// Временно для теста. Должен включаться после кат сцены
        WeaponIsHide = true;// Временно для теста. Должен включаться после кат сцены
        _canFire = true;// Временно для теста. Должен включаться после кат сцены
        _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    }
    private void Update()
    {
        if (IsControllable)
        {
            ChekCanJump();
            Scroll(ScrollSpeed);
            RotateWeapon();
            Shoot();
        }
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
        if (_canFire)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var tmpBullet = Instantiate(_bullet, ShootingPoint.position, _weaponTransform.rotation);
                tmpBullet.Speed = _bulletSpeed;
                _canFire = false;
                _timeAfterLastShot = 0;
            }
            if (Input.GetMouseButtonDown(1)&&CanHarpoon)
            {
                
                CanHarpoon = false;
                var tmpHarpoon = Instantiate(_harpoon, ShootingPoint.position, _weaponTransform.rotation);
                tmpHarpoon.ShootSpeed = _harpoonShootSpeed;
                tmpHarpoon.MaxShootDistance = _harpoonMaxShootDistance;
                tmpHarpoon.BackSpeed = _harpoonBackSpeed;
                _canFire = false;
                _timeAfterLastShot = 0;
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
            PickUp(collision.GetComponent<PickUp>().TypeOf);
        }
    }
    /// <summary>
    /// Срабатывает когда игрок подбирает бафф или дебафф или что душе угодно
    /// </summary>
    /// <param name="typeOf">Что подобрал</param>
    public void PickUp(PickUps typeOf)
    {
        switch (typeOf)
        {
            case PickUps.buff:
                Debug.Log("buff");
                break;
            case PickUps.debuff:
                Debug.Log("debuff");
                break;
            case PickUps.life:
                Debug.Log("life");
                break;
            case PickUps.weapon:
                Debug.Log("weapon");
                break;
            case PickUps.bullet:
                Debug.Log("bullet");
                break;
            case PickUps.bug:
                Debug.Log("bug");
                break;
            default:
                break;
        }
    }
}
