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
    [Range(1.5f, 8f)]
    private float _minXDistanceBetweenPlatforms = 2f;
    [SerializeField]
    [Range(1.5f, 10f)]
    private float _maxXDistanceBetweenPlatforms = 4f;
    [SerializeField]
    [Range(0f, 1f)]
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


    [Header("Стены")]

    [SerializeField]
    private int _sizeOfWallsObjectPool = 5;
    [SerializeField]
    private float _wallsYDistanceToPlatform = 3f;
    [SerializeField]
    private float _delayBeforeFirstWall = 30f;
    [SerializeField]
    private float _minDelayBetweenWalls = 25f;
    [SerializeField]
    private float _maxDelayBetweenWalls = 60f;
    [SerializeField]
    private GameObject _wallPrefab;


    [Header("Баффы на платформах")]

    [SerializeField]
    private int _sizeOfPickupBuffsOnPlatformsObjectPool = 5;
    [SerializeField]
    [Range(0.2f, 4f)]
    private float _buffOnPlatformYDistanceToPlatform = 3f;
    [SerializeField]
    private float _delayBeforeFirstBuffOnPlatforms = 10f;
    [SerializeField]
    private float _minDelayBetweenBuffOnPlatforms = 5f;
    [SerializeField]
    private float _maxDelayBetweenBuffOnPlatforms = 20f;
    [SerializeField]
    private GameObject _pickupBuffOnPlatformsPrefab;


    [Header("Дебаффы на платформах")]

    [SerializeField]
    private int _sizeOfPickupDebuffsOnPlatformsObjectPool = 5;
    [SerializeField]
    [Range(0.2f, 4f)]
    private float _debuffOnPlatformYDistanceToPlatform = 3f;
    [SerializeField]
    private float _delayBeforeFirstDebuffOnPlatforms = 10f;
    [SerializeField]
    private float _minDelayBetweenDebuffOnPlatforms = 5f;
    [SerializeField]
    private float _maxDelayBetweenDebuffOnPlatforms = 20f;
    [SerializeField]
    private GameObject _pickupDebuffOnPlatformsPrefab;


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


    [Header("Баффы в воздухе")]

    [SerializeField]
    private int _sizeOfBuffsInTheAirObjectPool = 15;
    [SerializeField]
    [Range(2f, 4f)]
    private float _minHeightAbovePlatformForBuffsInTheAir = 2f;
    [SerializeField]
    [Range(4f, 8f)]
    private float _maxHeightAbovePlatformForBuffsInTheAir = 5f;
    [SerializeField]
    private float _delayBeforeFirstBuffInTheAir = 10f;
    [SerializeField]
    private float _minDelayBetweenBuffsInTheAir = 1f;
    [SerializeField]
    private float _maxDelayBetweenBuffsInTheAir = 5f;
    [SerializeField]
    private GameObject _BuffInTheAirPrefab;


    [Header("Общие параметры фоновых элементов")]

    [SerializeField]
    [Range(5f, 10f)]
    private float _maxYDistanceOfBackgroundObjectsFromPlatform = 6f;
    [SerializeField]
    [Range(5f, 20f)]
    private float _backgroundObjectsBatchWidth = 10f;

    [Header("Элементы заднего фона дальние")]

    [SerializeField]
    private int _sizeOfFarBackgroundObjectsObjectPool = 200;
    [SerializeField]
    private int _minAmountOfFarBackgroundObjectsInBatch = 10;
    [SerializeField]
    private int _maxAmountOfFarBackgroundObjectsInBatch = 20;
    [SerializeField]
    private Sprite[] _farBackgroundObjectsSprites;
    [SerializeField]
    GameObject _farBackgroundObjectPrefab;
    
    [Header("Элементы заднего фона средние")]

    [SerializeField]
    private int _sizeOfMidBackgroundObjectsObjectPool = 200;
    [SerializeField]
    private int _minAmountOfMidBackgroundObjectsInBatch = 10;
    [SerializeField]
    private int _maxAmountOfMidBackgroundObjectsInBatch = 20;
    [SerializeField]
    private Sprite[] _midBackgroundObjectsSprites;
    [SerializeField]
    GameObject _midBackgroundObjectPrefab;

    [Header("Элементы заднего фона ближние")]

    [SerializeField]
    private int _sizeOfNearBackgroundObjectsObjectPool = 200;
    [SerializeField]
    private int _minAmountOfNearBackgroundObjectsInBatch = 10;
    [SerializeField]
    private int _maxAmountOfNearBackgroundObjectsInBatch = 20;
    [SerializeField]
    private Sprite[] _nearBackgroundObjectsSprites;
    [SerializeField]
    GameObject _nearBackgroundObjectPrefab;


    [Header("Элементы переднего фона")]

    [SerializeField]
    private int _sizeOfForegroundObjectsObjectPool = 200;
    [SerializeField]
    private int _minAmountOfForegroundObjectsInBatch = 10;
    [SerializeField]
    private int _maxAmountOfForegroundObjectsInBatch = 20;
    [SerializeField]
    private Sprite[] _foregroundObjectsSprites;
    [SerializeField]
    GameObject _foregroundObjectPrefab;




    //private float _platformPartPrefabWidth;

    private PlatformGenerator _platformsGenerator;

    private ObjectsLyingOnPlatformsGenerator _wallsGenerator;
    private ObjectsLyingOnPlatformsGenerator _buffsOnPlatformsGenerator;
    private ObjectsLyingOnPlatformsGenerator _debuffsOnPlatformsGenerator;

    private FloatingObjectsGenerator _bugsInTheAirGenerator;
    private FloatingObjectsGenerator _buffsInTheAirGenerator;

    private BackgroundElementsGenerator _farBackgroundObjectsGenerator;
    private BackgroundElementsGenerator _midBackgroundObjectsGenerator;
    private BackgroundElementsGenerator _nearBackgroundObjectsGenerator;
    private BackgroundElementsGenerator _foregroundObjectsGenerator;


    //private GameObject _tempPlatformPart;

    private PixelGridSnapper _gridSnapper;



    void Start()
    {
        _gridSnapper = FindObjectOfType<GameManager>().PixelGridSnapper;
               

        _wallsGenerator = new ObjectsLyingOnPlatformsGenerator(_sizeOfWallsObjectPool, transform, _wallPrefab, (int)(_delayBeforeFirstWall / _delayToCheckBuildAndRemoveObjects), (int)(_minDelayBetweenWalls / _delayToCheckBuildAndRemoveObjects), (int)(_maxDelayBetweenWalls / _delayToCheckBuildAndRemoveObjects), true);

        _buffsOnPlatformsGenerator = new ObjectsLyingOnPlatformsGenerator(_sizeOfPickupBuffsOnPlatformsObjectPool, transform, _pickupBuffOnPlatformsPrefab, (int)(_delayBeforeFirstBuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), (int)(_minDelayBetweenBuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), (int)(_maxDelayBetweenBuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), false);
        _debuffsOnPlatformsGenerator = new ObjectsLyingOnPlatformsGenerator(_sizeOfPickupDebuffsOnPlatformsObjectPool, transform, _pickupDebuffOnPlatformsPrefab, (int)(_delayBeforeFirstDebuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), (int)(_minDelayBetweenDebuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), (int)(_maxDelayBetweenDebuffOnPlatforms / _delayToCheckBuildAndRemoveObjects), false);
        
        _bugsInTheAirGenerator = new FloatingObjectsGenerator(_sizeOfBugsInTheAirObjectPool, transform, _BugInTheAirPrefab, (int)(_delayBeforeFirstBugInTheAir / _delayToCheckBuildAndRemoveObjects), (int)(_minDelayBetweenBugsInTheAir / _delayToCheckBuildAndRemoveObjects), (int)(_maxDelayBetweenBugsInTheAir / _delayToCheckBuildAndRemoveObjects));
        _buffsInTheAirGenerator = new FloatingObjectsGenerator(_sizeOfBuffsInTheAirObjectPool, transform, _BuffInTheAirPrefab, (int)(_delayBeforeFirstBuffInTheAir / _delayToCheckBuildAndRemoveObjects), (int)(_minDelayBetweenBuffsInTheAir / _delayToCheckBuildAndRemoveObjects), (int)(_maxDelayBetweenBuffsInTheAir / _delayToCheckBuildAndRemoveObjects));

        _farBackgroundObjectsGenerator = new BackgroundElementsGenerator(_sizeOfFarBackgroundObjectsObjectPool, transform, _farBackgroundObjectPrefab);
        _midBackgroundObjectsGenerator = new BackgroundElementsGenerator(_sizeOfMidBackgroundObjectsObjectPool, transform, _midBackgroundObjectPrefab);
        _nearBackgroundObjectsGenerator = new BackgroundElementsGenerator(_sizeOfNearBackgroundObjectsObjectPool, transform, _nearBackgroundObjectPrefab);
        _foregroundObjectsGenerator = new BackgroundElementsGenerator(_sizeOfForegroundObjectsObjectPool, transform, _foregroundObjectPrefab);



    }


    private IEnumerator LevelGenerationLoop()
    {

        _platformsGenerator = new PlatformGenerator(_sizeOfThePlatformPartsObjectPool, _sizeOfThePlatformsObjectPool, transform, _platformPartPrefab, _platformPrefab, _firstPlatformLength, _leftBorderX, _leftPlatformEdgeSprite, _platformMiddlePartsSprites, _rightPlatformEdgeSprite, _gridSnapper);

        _farBackgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_leftBorderX, _rightBorderX - _leftBorderX, _maxYDistanceOfBackgroundObjectsFromPlatform, (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _minAmountOfFarBackgroundObjectsInBatch), (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _maxAmountOfFarBackgroundObjectsInBatch), _farBackgroundObjectsSprites, _gridSnapper);
        _midBackgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_leftBorderX, _rightBorderX - _leftBorderX, _maxYDistanceOfBackgroundObjectsFromPlatform, (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _minAmountOfMidBackgroundObjectsInBatch), (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _maxAmountOfMidBackgroundObjectsInBatch), _midBackgroundObjectsSprites, _gridSnapper);
        _farBackgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_leftBorderX, _rightBorderX - _leftBorderX, _maxYDistanceOfBackgroundObjectsFromPlatform, (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _minAmountOfNearBackgroundObjectsInBatch), (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _maxAmountOfNearBackgroundObjectsInBatch), _nearBackgroundObjectsSprites, _gridSnapper);
        _farBackgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_leftBorderX, _rightBorderX - _leftBorderX, _maxYDistanceOfBackgroundObjectsFromPlatform, (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _minAmountOfForegroundObjectsInBatch), (int)(((_rightBorderX - _leftBorderX) / _backgroundObjectsBatchWidth) * _maxAmountOfForegroundObjectsInBatch), _foregroundObjectsSprites, _gridSnapper);


        WaitForSeconds delay = new WaitForSeconds(_delayToCheckBuildAndRemoveObjects);        

        while (true)
        {

            #region создаём объекты

            _platformsGenerator.CheckAndTryCreatePlatform(_rightBorderX, _minPlatformLength, _maxPlatformLength, _minXDistanceBetweenPlatforms, _maxXDistanceBetweenPlatforms, _minYDistanceBetweenPlatforms, _maxYDistanceBetweenPlatforms, _leftPlatformEdgeSprite, _platformMiddlePartsSprites, _rightPlatformEdgeSprite, _gridSnapper);

            _wallsGenerator.CheckAndTryCreateObjectOnPlatform(_rightBorderX, _wallsYDistanceToPlatform, _gridSnapper);

            _buffsOnPlatformsGenerator.CheckAndTryCreateObjectOnPlatform(_rightBorderX, _buffOnPlatformYDistanceToPlatform, _gridSnapper);
            _debuffsOnPlatformsGenerator.CheckAndTryCreateObjectOnPlatform(_rightBorderX, _debuffOnPlatformYDistanceToPlatform, _gridSnapper);

            _buffsInTheAirGenerator.CheckAndTryCreateFloatingObject(_rightBorderX, _minHeightAbovePlatformForBuffsInTheAir, _maxHeightAbovePlatformForBuffsInTheAir, _gridSnapper);
            _bugsInTheAirGenerator.CheckAndTryCreateFloatingObject(_rightBorderX, _minHeightAbovePlatformForBugsInTheAir, _maxHeightAbovePlatformForBugsInTheAir, _gridSnapper);
            
            _farBackgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_rightBorderX, _backgroundObjectsBatchWidth, _maxYDistanceOfBackgroundObjectsFromPlatform, _minAmountOfFarBackgroundObjectsInBatch, _maxAmountOfFarBackgroundObjectsInBatch, _farBackgroundObjectsSprites, _gridSnapper);
            _midBackgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_rightBorderX, _backgroundObjectsBatchWidth, _maxYDistanceOfBackgroundObjectsFromPlatform, _minAmountOfMidBackgroundObjectsInBatch, _maxAmountOfMidBackgroundObjectsInBatch, _midBackgroundObjectsSprites, _gridSnapper);
            _nearBackgroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_rightBorderX, _backgroundObjectsBatchWidth, _maxYDistanceOfBackgroundObjectsFromPlatform, _minAmountOfNearBackgroundObjectsInBatch, _maxAmountOfNearBackgroundObjectsInBatch, _nearBackgroundObjectsSprites, _gridSnapper);
            _foregroundObjectsGenerator.CheckAndTryCreateBatchOfObjects(_rightBorderX, _backgroundObjectsBatchWidth, _maxYDistanceOfBackgroundObjectsFromPlatform, _minAmountOfForegroundObjectsInBatch, _maxAmountOfForegroundObjectsInBatch, _foregroundObjectsSprites, _gridSnapper);

            #endregion

            #region уничтожаем объекты

            _platformsGenerator.CheckAndTryRemoveObjects(_leftBorderX);

            _wallsGenerator.CheckAndTryRemoveObjects(_leftBorderX);

            _buffsOnPlatformsGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _debuffsOnPlatformsGenerator.CheckAndTryRemoveObjects(_leftBorderX);

            _buffsInTheAirGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _bugsInTheAirGenerator.CheckAndTryRemoveObjects(_leftBorderX);

            _farBackgroundObjectsGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _midBackgroundObjectsGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _nearBackgroundObjectsGenerator.CheckAndTryRemoveObjects(_leftBorderX);
            _foregroundObjectsGenerator.CheckAndTryRemoveObjects(_leftBorderX);


            #endregion


            yield return delay;
        }
    }

    public void StartGeneration()
    {
        StartCoroutine(LevelGenerationLoop());
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
