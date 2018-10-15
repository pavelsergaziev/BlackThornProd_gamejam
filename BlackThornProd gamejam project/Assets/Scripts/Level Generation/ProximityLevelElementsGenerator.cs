using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProximityLevelElementsGenerator : BaseLevelElementsGenerator
{
    public ProximityLevelElementsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab) : base(objectsPoolSize, levelLayoutObject, objectPrefab)
    {
    }

    public virtual void CheckAndTryCreateObject(int creatingBorderPositionX, float minXDistanceToNextObject, float maxXDistanceToNextObject, float minYPosition, float maxYPosition, PixelGridSnapper gridSnapper)
    {
        Transform rightmostActiveObject = _activeObjects[_activeObjects.Count - 1].transform;

        if (rightmostActiveObject.position.x < creatingBorderPositionX)
            PlaceObjectFromPool(new Vector3(rightmostActiveObject.position.x + Random.Range(minXDistanceToNextObject, maxXDistanceToNextObject), rightmostActiveObject.position.y + Random.Range(minYPosition, maxYPosition), _objectsPool.Peek().transform.position.z), gridSnapper);
    }
}
