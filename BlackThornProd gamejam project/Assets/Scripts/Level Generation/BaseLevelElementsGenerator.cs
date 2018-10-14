using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevelElementsGenerator {    

    protected Queue<GameObject> _objectsPool = new Queue<GameObject>();
    protected List<GameObject> _activeObjects = new List<GameObject>();
    
    protected GameObject _tempObject;

    public BaseLevelElementsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab)
    {
        for (int i = 0; i < objectsPoolSize; i++)
        {
            _tempObject = Object.Instantiate(objectPrefab, levelLayoutObject.position, Quaternion.identity, levelLayoutObject);
            _tempObject.SetActive(false);
            _objectsPool.Enqueue(_tempObject);
        }
        
    }

    public virtual void PlaceFirstObject(Vector3 position, PixelGridSnapper gridSnapper)
    {
        PlaceObjectFromPool(position, gridSnapper);
    }

    public virtual void CheckAndTryRemoveObjects(int destroyingBorderPositionX)
    {
        while (_activeObjects[0].transform.position.x < destroyingBorderPositionX)
            DeactivateObjectAndPutItIntoPool();
    }

    public virtual void CheckAndTryCreateObject(int creatingBorderPositionX, float minXDistanceToNextObject, float maxXDistanceToNextObject, float minYPosition, float maxYPosition, PixelGridSnapper gridSnapper)
    {
        Transform rightmostActiveObject = _activeObjects[_activeObjects.Count - 1].transform;

        if (rightmostActiveObject.position.x < creatingBorderPositionX)
            PlaceObjectFromPool(new Vector3(rightmostActiveObject.position.x + Random.Range(minXDistanceToNextObject, maxXDistanceToNextObject), rightmostActiveObject.position.y + Random.Range(minYPosition, maxYPosition), _objectsPool.Peek().transform.position.z), gridSnapper);
    }


    protected virtual void PlaceObjectFromPool(Vector3 position, PixelGridSnapper gridSnapper)
    {
        _tempObject = _objectsPool.Dequeue();
        _tempObject.transform.position = position;
        gridSnapper.SnapToTexelGrid(_tempObject.transform.GetComponentInChildren<SpriteRenderer>().transform, _tempObject.transform);
        _tempObject.SetActive(true);
        _activeObjects.Add(_tempObject);
    }

    protected virtual void PlaceObjectFromPool(Vector3 position, Sprite sprite, PixelGridSnapper gridSnapper)
    {
        PlaceObjectFromPool(position, gridSnapper);
        _tempObject.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }

    protected virtual void DeactivateObjectAndPutItIntoPool()
    {
        _tempObject = _activeObjects[0];
        _activeObjects.RemoveAt(0);
        _objectsPool.Enqueue(_tempObject);
        _tempObject.SetActive(false);
    }
    
}
