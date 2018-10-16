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
    private const float _destroyTime = 3f;
    private PlayerController _playerController;
    private void Start()
    {
        DestroyBullet(_destroyTime);
        _playerController = FindObjectOfType<PlayerController>();
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
        Destroy(gameObject);
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
        
        if (collision.tag == "PickUp" )
        {
            var collisionScript = collision.GetComponent<ScrollingGameObject>();
            collisionScript.SwitchVisibility();
            //применить штраф за уничтожение бафа или дать бонус за уничтожение бага
            DestroyBullet();
        }
    }
}

