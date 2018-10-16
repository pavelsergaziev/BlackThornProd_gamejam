using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationController : MonoBehaviour {

    [SerializeField]
    [Tooltip("Расстояние в юнитах от центра геймобжекта до левого края, за которым объекты уничтожаются")]
    private int _leftBorderX = - 10;
    [SerializeField]
    [Tooltip("Расстояние в юнитах от центра геймобжекта до правого края, за которым объекты создаются")]
    private int _rightBorderX = 10;

    [SerializeField]
    [Tooltip("Задержка между проверками создания/уничтожения объектов")]
    private float _delayToCheckBuildAndRemoveObjects = 1;

    //вероятно, лучше эти числа получить из отношения размеров экрана к размерам тайлов, но пока вобьём руками
    [Header("Платформы")]

    [SerializeField]
    private int _sizeOfThePlatformPartsObjectPool;
    [SerializeField]
    private int _sizeOfThePlatformsObjectPool;
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
    private Sprite _leftPlatformEdgeSprite;
    [SerializeField]
    private Sprite _rightPlatformEdgeSprite;
    [SerializeField]
    private Sprite[] _platformMiddlePartsSprites;

    [SerializeField]
    private GameObject _platformPartPrefab;

    [SerializeField]
    private GameObject _platformPrefab;


    [SerializeField]
    private GameObject _testPickupPrefab;



    [Header("Баффы на платформах")]

    [SerializeField]
    private int _sizeOfPickupBuffsOnPlatformsObjectPool = 5;
    [SerializeField]
    [Range(0.2f, 2f)]
    private float _buffOnPlatformYDistanceToPlatform = 1f;
    [SerializeField]
    private float _delayBeforeFirstBuffOnPlatforms = 10f;
    [SerializeField]
    private float _minDelayBetweenBuffOnPlatforms = 5f;
    [SerializeField]
    private float _maxDelayBetweenBuffOnPlatforms = 20f;
    [SerializeField]
    private GameObject _pickupBuffOnPlatformsPrefab;

    [Header("Баги в воздухе")]

    [SerializeField]
    private int _sizeOfBugsInTheAirObjectPool = 15;
    [SerializeField]
    [Range(2f, 4f)]
    private float _minHeightAbovePlatformForBugsInTheAir = 2f;
    [SerializeField]
    [Range(4f, 8f)]
    private float _maxHeightAbovePlatformForBugsInTheAir = 5f;
    [SerializeField]
    private float _delayBeforeFirstBugInTheAir = 10f;
    [SerializeField]
    private float _minDelayBetweenBugsInTheAir = 1f;
    [SerializeField]
    private float _maxDelayBetweenBugsInTheAir = 5f;
    [SerializeField]
    private GameObject _BugInTheAirPrefab;
       
    [Header("Элементы заднего фона")]
    [SerializeField]
    private int _sizeOfBackgroundObjectsObjectPool = 200;
    [SerializeField]
    [Range(5f, 20f)]
    private float _backgroundObjectsBatchWidth = 10f;
    [SerializeField]
    [Range(5f, 10f)]
    private float _maxYDistanceOfBackgroundObjectsFromPlatform = 6f;
    [SerializeField]
    private int _minAmountOfBackgroundObjectsInBatch = 10;
    [SerializeField]
    private int _maxAmountOfBackgroundObjectsInBatch = 20;
    [SerializeField]
    private Sprite[] _backgroundObjectsSprites;
    [SerializeField]
    GameObject _backgroundObjectPrefab;



    //private float _platformPartPrefabWidth;

    private PlatformGenerator _platformsGenerator;
    private ObjectsLyingOnPlatformsGenerator _buffsOnPlatformsGenerator;
    private FloatingObjectsGenerator _bugsInTheAirGenerator;
    private BackgroundElementsGenerator _backgroundObjectsGenerator;


    private GameObject _tempPlatformPart;

    private PixelGridSnapper _gridSnapper;



    void Start()
    {
        _gridSnapper = FindObjectOfType<GameManager>().PixelGridSnapper;

        _platformsGenerator = new PlatformGenerator(_sizeOfThePlatformPartsObjectPool, _sizeOfThePlatformsObjectPool, transform, _platformPartPrefab, _platformPrefab, _firstPlatformLength, _leftBorderX, _leftPlatformEdgeSprite, _platformMiddlePartsSprites, _rightPlatformEdgeSprite, _gridSnapper);
        _buffsOnPlatformsGenerator = new ObjectsLyingOnPlatformsGenerator(_sizeOfPickupBuffsOnPlatformsObjectPool, transform, _pickupBuffOnPlatformsPrefab, (int)(_delayBeforeFirstBuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), (int)(_minDelayBetweenBuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), (int)(_maxDelayBetweenBuffOnPlatforms / _delayToCheckBuildAndRemoveObjects));
        _bugsInTheAirGenerator = new FloatingObjectsGenerator(_sizeOfBugsInTheAirObjectPool, transform, _BugInTheAirPrefab, (int)(_delayBeforeFirstBugInTheAir / _delayToCheckBuildAndRemoveObjects), (int)(_minDelayBetweenBugsInTheAir / _delayToCheckBuildAndRemoveObjects), (int)(_maxDelayBetweenBugsInTheAir / _delayToCheckBuildAndRemoveObjects));
        _backgroundObjectsGenerator = new BackgroundElementsGenerator(_sizeOfBackgroundObjectsObjectPool, transform, _backgroundObjectPrefab);


        _backgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_leftBorderX, _rightBorderX - _leftBorderX, _maxYDistanceOfBackgroundObjectsFromPlatform, (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _minAmountOfBackgroundObjectsInBatch), (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _maxAmountOfBackgroundObjectsInBatch), _backgroundObjectsSprites, _gridSnapper);
        StartCoroutine(LevelGenerationLoop());
    }
     

    private IEnumerator LevelGenerationLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayToCheckBuildAndRemoveObjects);        

        while (true)
        {

            _platformsGenerator.CheckAndTryCreatePlatform(_rightBorderX, _minPlatformLength, _maxPlatformLength, _minXDistanceBetweenPlatforms, _maxXDistanceBetweenPlatforms, _minYDistanceBetweenPlatforms, _maxYDistanceBetweenPlatforms, _leftPlatformEdgeSprite, _platformMiddlePartsSprites, _rightPlatformEdgeSprite, _gridSnapper);
            _buffsOnPlatformsGenerator.CheckAndTryCreateObjectOnPlatform(_rightBorderX, _buffOnPlatformYDistanceToPlatform, _gridSnapper);
            _bugsInTheAirGenerator.CheckAndTryCreateFloatingObject(_rightBorderX, _minHeightAbovePlatformForBugsInTheAir, _maxHeightAbovePlatformForBugsInTheAir, _gridSnapper);            
            _backgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_rightBorderX, _backgroundObjectsBatchWidth, _maxYDistanceOfBackgroundObjectsFromPlatform, _minAmountOfBackgroundObjectsInBatch, _maxAmountOfBackgroundObjectsInBatch, _backgroundObjectsSprites, _gridSnapper);


            _platformsGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _buffsOnPlatformsGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _bugsInTheAirGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _backgroundObjectsGenerator.CheckAndTryRemoveObjects(_leftBorderX);


            yield return delay;
        }
    }
    

    //а это так, посмотреть, как рандом работает в Юнити и как можно организовать точное воспроизведение уровня через "сид", точнее через стейт.
    public void TestRandom()
    {

        Random.InitState(12345);//задаём стейт числом

        Debug.Log("Первое число - " + Random.Range(0, 1001));
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


}
