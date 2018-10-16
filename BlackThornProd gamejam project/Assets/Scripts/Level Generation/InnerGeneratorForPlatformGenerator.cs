using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerGeneratorForPlatformGenerator : BaseLevelElementsGenerator {
    public InnerGeneratorForPlatformGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, float platformScrollSpeed) : base(objectsPoolSize, levelLayoutObject, objectPrefab, platformScrollSpeed)
    {
    }

    public GameObject NextPlatformInPool { get { return _objectsPool.Peek(); } }

    public void PlacePlatformFromPool(Vector3 position, Vector2 colliderSize)
    {
        GameObject _platform = _objectsPool.Peek();
        BoxCollider2D collider = _platform.GetComponent<BoxCollider2D>();
        collider.size = colliderSize;
        PlaceObjectFromPool(position);
    }

    public override void CheckAndTryRemoveObjects(int destroyingBorderPositionX)
    {
        if (_activeObjects.Count > 0 && _activeObjects[0].transform.position.x + _activeObjects[0].GetComponent<BoxCollider2D>().size.x / 2 < destroyingBorderPositionX)
            DeactivateObjectAndPutItIntoPool();
    }
}
