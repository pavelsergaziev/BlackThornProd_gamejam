using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElementsGenerator : ProximityLevelElementsGenerator {

    private const int MaxSortingOrder = 300;
    private const float YPlus = 4;


    public BackgroundElementsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab) : base(objectsPoolSize, levelLayoutObject, objectPrefab)
    {
    }

    public void CheckAndTryCreateBatchOfObjects(int creatingBorderPositionX, float maxXDistanceToOriginPoint, float maxYDistanceFromPlatform, int minAmountOfObjectsInBatch, int maxAmountOfObjectsInBatch, Sprite[] sprites, PixelGridSnapper gridSnapper)
    {
        Debug.Log(_activeObjects.Count);

        if (_activeObjects.Count > 0)
            Debug.Log(_activeObjects[_activeObjects.Count - 1].name + " " + (_activeObjects[_activeObjects.Count - 1].transform.position.x - creatingBorderPositionX));

        if (_activeObjects.Count == 0 || _activeObjects[_activeObjects.Count - 1].transform.position.x < creatingBorderPositionX)
        {
            Vector2 raycastSource = new Vector2(creatingBorderPositionX, 0);

            RaycastHit2D hit = Physics2D.Raycast(raycastSource, Vector2.down, RaycastDistance, LayerMaskForRaycasting);
            if (!hit)
                hit = Physics2D.Raycast(raycastSource, Vector2.up);

            if (_activeObjects.Count > 0 && hit)
                Debug.Log(_activeObjects[_activeObjects.Count - 1].name + " hit w raycast " + hit.collider.tag);

            if (hit && hit.collider.tag == "Platform")
            {
                Debug.Log("зашли в условие хита");
                int amountOfObjects = Random.Range(minAmountOfObjectsInBatch, maxAmountOfObjectsInBatch + 1);
                float xDistanceBetweenObjects = maxXDistanceToOriginPoint / amountOfObjects;

                for (int i = 0; i < amountOfObjects; i++)
                { 
                    PlaceObjectFromPool
                    (
                        new Vector3(
                            creatingBorderPositionX + (xDistanceBetweenObjects * i),
                            hit.collider.transform.position.y - maxYDistanceFromPlatform + Random.Range(0, maxYDistanceFromPlatform * 2) + YPlus,
                            _objectsPool.Peek().transform.position.z),
                        sprites[Random.Range(0, sprites.Length)],
                        gridSnapper
                    );

                    if (_activeObjects.Count > 1)
                    {
                        SpriteRenderer previousObjectSpriteRenderer = _activeObjects[_activeObjects.Count - 2].GetComponentInChildren<SpriteRenderer>();

                        if (previousObjectSpriteRenderer.sortingOrder > MaxSortingOrder)
                            previousObjectSpriteRenderer.sortingOrder = 0;

                        _activeObjects[_activeObjects.Count - 1].GetComponentInChildren<SpriteRenderer>().sortingOrder = previousObjectSpriteRenderer.sortingOrder + 1;
                    }

                }


            }
        }
    }

}
