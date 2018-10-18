using UnityEngine;
/// <summary>
/// Содержит логику и поля пули
/// </summary>
class Bullet : MonoBehaviour
{
    /// <summary>
    /// Скорость пули
    /// </summary>
    [HideInInspector]
    public float Speed;
    /// <summary>
    /// Время, через которое снаряд будет уничтожен, если ни во что не врежется
    /// </summary>
    [SerializeField]
    private float _destroyTime = 1.5f;

    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private int _explosionObjectDestroyTime;


    private PlayerController _playerController;
    
    private SoundOnObject _soundController;
    
    [SerializeField]
    private float _rocketLounchVolume;
    [SerializeField]
    private float _explosionVolume;
    

    private void Start()
    {
        DestroyBullet(_destroyTime);
        _playerController = FindObjectOfType<PlayerController>();
        _soundController = GetComponent<SoundOnObject>();
        _soundController.PlaySound("RocketShot",false, _rocketLounchVolume);
    }
    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }
    /// <summary>
    /// Логика уничтожения пули
    /// </summary>
    private void DestroyBullet()
    {
        //анимация взрыва
        _soundController.PlaySound("RocketExplosion",false, _explosionVolume);
        GetComponent<SpriteRenderer>().enabled = false;
        Speed = 0;
        foreach (var item in GetComponents<Collider2D>())
        {
            item.enabled = false;
        }
        Destroy(gameObject,_soundController.GetTimeToEndOfClip("RocketExplosion"));
        return;
    }
    /// <summary>
    /// логика уничтожения пули через промежуток времени
    /// </summary>
    /// <param name="time">Время до уничтожения</param>
    private void DestroyBullet(float time)
    {
        //анимация взрыва
        Destroy(gameObject, time);
        return;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.tag == "PickUp" || collision.tag == "Destractable")
        {

            //анимация взрыва
            //положил сюда, но в зависимости от того, чем мы стреляем, может, и перенести в destroy bullet
            Destroy(Instantiate(_explosionPrefab, transform.position, Quaternion.identity), _explosionObjectDestroyTime);


            var collisionScript = collision.GetComponent<ScrollingGameObject>();
            switch (collisionScript.TypeOf)
            {
                case TypeOfObject.none:
                    break;
                case TypeOfObject.buff:
                    Debug.Log("bullet destroy buff");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.debuff:
                    Debug.Log("bullet destroy debuff");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.life:
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.weapon:
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.bullet:
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.bug:
                    Debug.Log("bullet destroy bug");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;
                case TypeOfObject.destructable:
                    Debug.Log("bullet destroy Wall");
                    collisionScript.SwitchVisibility();
                    DestroyBullet();
                    break;

            }
        }
        
    }
}

