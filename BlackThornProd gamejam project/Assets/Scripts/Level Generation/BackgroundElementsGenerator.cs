using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElementsGenerator : ProximityLevelElementsGenerator {
    public BackgroundElementsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab) : base(objectsPoolSize, levelLayoutObject, objectPrefab)
    {
    }

    public void CheckAndTryCreateBatchOfObjects(int creatingBorderPositionX, float maxXDistanceToOriginPoint, float maxYDistanceFromPlatform, int minAmountOfObjectsInBatch, int maxAmountOfObjectsInBatch, Sprite[] sprites, PixelGridSnapper gridSnapper)
    {
        if (_activeObjects.Count == 0 || _activeObjects[_activeObjects.Count - 1].transform.position.x < creatingBorderPositionX)
        {
            Vector2 raycastSource = new Vector2(creatingBorderPositionX, 0);

            RaycastHit2D hit = Physics2D.Raycast(raycastSource, Vector2.down);
            if (!hit)
                hit = Physics2D.Raycast(raycastSource, Vector2.up);

            if (hit && hit.collider.tag == "Platform")
            {
                Debug.Log(hit.transform.position);

                int amountOfObjects = Random.Range(minAmountOfObjectsInBatch, maxAmountOfObjectsInBatch + 1);
                float xDistanceBetweenObjects = maxXDistanceToOriginPoint / amountOfObjects;

                Debug.Log(amountOfObjects);
                Debug.Log(xDistanceBetweenObjects);

                for (int i = 0; i < amountOfObjects; i++)
                    PlaceObjectFromPool
                    (
                        new Vector3(
                            creatingBorderPositionX + (xDistanceBetweenObjects * i),
                            hit.collider.transform.position.y - maxYDistanceFromPlatform + Random.Range(0, maxYDistanceFromPlatform * 2),
                            _objectsPool.Peek().transform.position.z),
                        sprites[Random.Range(0, sprites.Length)],
                        gridSnapper
                    );
            }
        }
    }


}
