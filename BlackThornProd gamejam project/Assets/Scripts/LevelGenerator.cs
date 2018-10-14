using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private int _distanceToABorder;

    //вероятно, лучше эти числа получить из отношения размеров экрана к размерам тайлов, но пока вобьём руками
    [SerializeField]
    private int _sizeOfThePlatformPartsObjectPool;
    [SerializeField]
    [Range(2,10)]
    private int _minPlatformLength = 3;
    [SerializeField]
    [Range(3, 20)]
    private int _maxPlatformLength = 10;
    [SerializeField]
    private int _firstPlatformLength = 5;

    [SerializeField]
    [Range(1.5f, 2f)]
    private float _minXDistanceBetweenPlatforms = 1.5f;
    [SerializeField]
    [Range(2f, 4f)]
    private float _maxXDistanceBetweenPlatforms = 3f;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float _minYDistanceBetweenPlatforms = 0.2f;
    [SerializeField]
    [Range(1f, 3f)]
    private float _maxYDistanceBetweenPlatforms = 2f;

    [SerializeField]
    private float _delayToCheckBuildAndRemovePlatforms = 1;


    [SerializeField]
    private Sprite _leftPlatformEdgeSprite;
    [SerializeField]
    private Sprite _rightPlatformEdgeSprite;
    [SerializeField]
    private Sprite[] _platformMiddlePartsSprites;

    [SerializeField]
    private GameObject _platformPartPrefab;

    private float _platformPartPrefabWidth;

    private Queue<GameObject> _platformPartsPool = new Queue<GameObject>();

    private List<GameObject> _activePlatformParts = new List<GameObject>();
    

    private GameObject _tempPlatformPart;

    void Start()
    {
        if (_minPlatformLength > _maxPlatformLength)
            throw new System.ArgumentOutOfRangeException("Минимальная длина платформы превосходит максимальную. Поправьте соответствующие поля в инспекторе LevelGenerator!");

        

        //var platformtest = Instantiate(_platformPartPrefab, Vector3.zero, Quaternion.identity);
        //platformtest.GetComponentInChildren<SpriteRenderer>().sprite = _leftPlatformEdgeSprite;


        Vector3 poolPosition = new Vector3(-30, 0, 0);
        for (int i = 0; i < _sizeOfThePlatformPartsObjectPool; i++)
        {
            _tempPlatformPart = Instantiate(_platformPartPrefab, poolPosition, Quaternion.identity, transform);
            _tempPlatformPart.SetActive(false);
            _platformPartsPool.Enqueue(_tempPlatformPart);
        }
        

        _platformPartPrefabWidth = _platformPartPrefab.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.x;

        CreatePlatform(_firstPlatformLength, transform.position);
        

        StartCoroutine(PlatformGenerationLoop());
    }



    public void TestRandom()
    {

        Random.InitState(12345);//задаём стейт числом

        Debug.Log("Первое число - " + Random.Range(0,1001));
        Debug.Log("Второе число - " + Random.Range(0, 1001));
        Debug.Log("Третье число - " + Random.Range(0, 1001));

        Random.InitState(12345);//задаём стейт числом

        Debug.Log("Первое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Второе число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Третье число другая формула - " + Random.value);

        Random.State state = Random.state;//сохраняем стейт

        Debug.Log("Новое первое число - " + Random.Range(0, 1001));
        Debug.Log("Новое торое число - " + Random.Range(0, 1001));
        Debug.Log("Новое третье число другая формула - " + Random.value);

        Random.state = state;//передаём сохранённый стейт

        Debug.Log("Новое первое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое торое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое третье число другая формула ещё раз - " + Random.value);

        //делаем рандомный стейт
        string timeTemp = System.DateTime.Now.Ticks.ToString();
        Random.InitState(System.Convert.ToInt16(timeTemp.Substring(timeTemp.Length - 4)));

        Debug.Log("Новое первое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое торое число ещё раз - " + Random.Range(0, 1001));
        Debug.Log("Новое третье число другая формула ещё раз - " + Random.value);
    }



    private IEnumerator PlatformGenerationLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayToCheckBuildAndRemovePlatforms);

        while (true)
        {

            CheckAndTryCreatePlatform(_activePlatformParts[_activePlatformParts.Count - 1].transform);
            CheckAndTryRemovePlatformPart(_activePlatformParts[0].transform);
            yield return delay;
        }
    }


    private void CheckAndTryRemovePlatformPart(Transform leftmostActivePlatformPart)
    {
        Debug.Log("для удаления " + leftmostActivePlatformPart.position.x + " меньше ли чем " + (transform.position.x - _distanceToABorder));
        if (leftmostActivePlatformPart.position.x < transform.position.x - _distanceToABorder)
            DeactivatePlatformPartAndPutItIntoPool();
    }

    private void CheckAndTryCreatePlatform(Transform rightmostActivePlatformPart)
    {
        Debug.Log("Для оздания" + rightmostActivePlatformPart.position.x + " меньше ли чем " + (transform.position.x + _distanceToABorder));
        if (rightmostActivePlatformPart.position.x < transform.position.x + _distanceToABorder)
            CreatePlatform(Random.Range(_minPlatformLength, _maxPlatformLength + 1), new Vector3(rightmostActivePlatformPart.position.x + Random.Range(_minXDistanceBetweenPlatforms, _maxXDistanceBetweenPlatforms), rightmostActivePlatformPart.position.y + (Random.Range(_minYDistanceBetweenPlatforms, _maxYDistanceBetweenPlatforms) * Random.Range(-1, 2)), transform.position.z));
    }

    private void CreatePlatform(int platformLength, Vector3 startingPosition)
    {
        PlacePlatformPartFromPool(_leftPlatformEdgeSprite, startingPosition);

        for (int i = 0; i < platformLength - 2; i++)
            PlacePlatformPartFromPool(_platformMiddlePartsSprites[Random.Range(0, _platformMiddlePartsSprites.Length)], new Vector3(startingPosition.x + ((i + 1) * _platformPartPrefabWidth), startingPosition.y, startingPosition.z));

        PlacePlatformPartFromPool(_rightPlatformEdgeSprite, new Vector3(startingPosition.x + ((platformLength - 1) * _platformPartPrefabWidth), startingPosition.y, startingPosition.z));
    }

    private void PlacePlatformPartFromPool(Sprite sprite, Vector3 position)
    {
        _tempPlatformPart = _platformPartsPool.Dequeue();
        _tempPlatformPart.transform.position = position;
        _tempPlatformPart.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        _tempPlatformPart.SetActive(true);
        _activePlatformParts.Add(_tempPlatformPart);
    }

    private void DeactivatePlatformPartAndPutItIntoPool()
    {
        Debug.Log("Зашли в удаление");
        _tempPlatformPart = _activePlatformParts[0];
        _activePlatformParts.RemoveAt(0);
        _platformPartsPool.Enqueue(_tempPlatformPart);
        _tempPlatformPart.SetActive(false);        
    }
    
}
