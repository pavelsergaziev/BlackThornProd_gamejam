using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Логика и поля гарпуна
/// </summary>
public class Harpoon : MonoBehaviour
{
    /// <summary>
    /// Скорость стрельбы
    /// </summary>
    [HideInInspector]
    public float ShootSpeed;
    /// <summary>
    /// Дальность стрельбы
    /// </summary>
    [HideInInspector]
    public float MaxShootDistance;
    /// <summary>
    /// Скорость возвращения
    /// </summary>
    [HideInInspector]
    public float  BackSpeed;
    /// <summary>
    /// Ссылка на компонент LineRenderer
    /// </summary>
    private LineRenderer _linerRenderer;
    /// <summary>
    /// Ссылка на игрока
    /// </summary>
    private PlayerController _player;
    /// <summary>
    /// Гарпун движется к цели?
    /// </summary>
    private bool _moveToTarget = true;
    /// <summary>
    /// Дистанция, на которой гарпун уничтожеатся при возвращении к игроку
    /// </summary>
    private const float _harpoonDestroyDIstance = 0.1f;
    /// <summary>
    /// Зацепил обьект PickUp? Служит для того, что бы  не зацепить несколько обьектов сразу
    /// </summary>
    private bool _pickUpisCatched = false;
    /// <summary>
    /// Тип пойманого обьекта
    /// </summary>
    private PickUps _typeOfCathedPickUp;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _linerRenderer = GetComponent<LineRenderer>();       
    }
    void Update ()
    {
        if (_moveToTarget)
        {
            transform.Translate(Vector2.right * ShootSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _player.ShootingPoint.position)>= MaxShootDistance)
            {
                _moveToTarget = false;
            }
        }
        if (!_moveToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position,_player.ShootingPoint.position, BackSpeed*Time.deltaTime);
            if (Vector2.Distance(transform.position, _player.ShootingPoint.position) <= _harpoonDestroyDIstance)
            {
                DestroyHarpoon();
            }
        }
        _linerRenderer.SetPosition(1,_player.ShootingPoint.position);
        _linerRenderer.SetPosition(0, transform.position);

    }
    /// <summary>
    /// Логика уничтожения гарпуна
    /// </summary>
    public void DestroyHarpoon()
    {
        _player.CanHarpoon = true;
        if (_pickUpisCatched)
        {
            _player.PickUp(_typeOfCathedPickUp);
        }
        Destroy(gameObject);///Здесь уничтожается подобранный обьект
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "PickUp"&& _pickUpisCatched == false)
        {
            ///Здесь подбирается зацепленный обьект и становится дочерним
            //я всё-таки сделал с копированием объекта. Так реально получается сильно меньше кода.
            GameObject affectedObject = collision.gameObject;
            GameObject pickedUpObjectClone = Instantiate(affectedObject, affectedObject.transform.position, Quaternion.identity, transform);
            pickedUpObjectClone.GetComponent<ScrollingGameObject>().ScrollSpeed = 0;
            affectedObject.GetComponent<ScrollingGameObject>().SwitchVisibility();
            
            _moveToTarget = false;
            _pickUpisCatched = true;
            _typeOfCathedPickUp = collision.GetComponent<PickUp>().TypeOf;
        }
    }
    
}
