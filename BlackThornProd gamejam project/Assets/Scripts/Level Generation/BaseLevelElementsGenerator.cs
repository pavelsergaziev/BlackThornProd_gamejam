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

    public BaseLevelElementsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, float objectScrollSpeed)
    {
        for (int i = 0; i < objectsPoolSize; i++)
        {
            _tempObject = Object.Instantiate(objectPrefab, levelLayoutObject.position, Quaternion.identity, levelLayoutObject);
            _tempObject.GetComponent<ScrollingGameObject>().ScrollSpeed = objectScrollSpeed;
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
        while (_activeObjects.Count > 0 && _activeObjects[0].transform.position.x < destroyingBorderPositionX)
            DeactivateObjectAndPutItIntoPool();
    }

    protected virtual void PlaceObjectFromPool(Vector3 position)
    {
        _tempObject = _objectsPool.Dequeue();
        _tempObject.transform.position = position;
        _tempObject.SetActive(true);        
        _activeObjects.Add(_tempObject);
    }

    protected virtual void PlaceObjectFromPool(Vector3 position, PixelGridSnapper gridSnapper)
    {        

        PlaceObjectFromPool(position);

        ScrollingGameObject tempScrollingObject = _tempObject.GetComponent<ScrollingGameObject>();

        if (!tempScrollingObject.ChildTransformToSnapToGrid.gameObject.activeSelf)
            tempScrollingObject.SwitchVisibility();

        gridSnapper.SnapToTexelGrid(tempScrollingObject.ChildTransformToSnapToGrid, _tempObject.transform);
    }

    protected virtual void PlaceObjectFromPool(Vector3 position, Sprite sprite, PixelGridSnapper gridSnapper)
    {
        PlaceObjectFromPool(position, gridSnapper);
        _tempObject.GetComponent<ScrollingGameObject>().ChildTransformToSnapToGrid.GetComponent<SpriteRenderer>().sprite = sprite;
    }    

    protected virtual void DeactivateObjectAndPutItIntoPool()
    {
        _tempObject = _activeObjects[0];
        _activeObjects.RemoveAt(0);
        _objectsPool.Enqueue(_tempObject);
        _tempObject.SetActive(false);
    }
    
}
