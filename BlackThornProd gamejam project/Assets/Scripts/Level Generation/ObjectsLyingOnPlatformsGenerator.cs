using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Генератор объектов, которые непосредственно лежат на платформах.
/// </summary>
public class ObjectsLyingOnPlatformsGenerator : TimedLevelElementsGenerator
{
    bool _placeObjectsOnlyAtPlatformCenter;

    private const int AttemptsToPlaceObject = 10;//просто какой-то предел, чтобы в бесконечный цикл не попасть

    public ObjectsLyingOnPlatformsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, int delayBeforeFirstPlacement, int minDelayBetweenObjectPlacements, int maxDelayBetweenObjectPlacements, bool placeObjectsOnlyAtPlatformCenter) : base(objectsPoolSize, levelLayoutObject, objectPrefab, delayBeforeFirstPlacement, minDelayBetweenObjectPlacements, maxDelayBetweenObjectPlacements)
    {
        _placeObjectsOnlyAtPlatformCenter = placeObjectsOnlyAtPlatformCenter;
    }

    /// <summary>
    /// Попробовать создать объект, лежащий на платформе.
    /// </summary>
    /// <param name="creatingBorderPositionX"></param>
    /// <param name="minXDistanceToNextObject"></param>
    /// <param name="maxXDistanceToNextObject"></param>
    /// <param name="gridSnapper"></param>
    public void CheckAndTryCreateObjectOnPlatform(int creatingBorderPositionX, float YDistanceToPlatform, PixelGridSnapper gridSnapper)
    {
        if (CheckIsItTimeToPlaceObject())
        {
            for (int i = 0; i < AttemptsToPlaceObject; i++)//просто какой-то предел, чтобы в бесконечный цикл не попасть
            {

                Vector2 raycastSource = new Vector2(creatingBorderPositionX + (i / 2), 0);

                Collider2D hitCollider = Physics2D.Raycast(raycastSource, Vector2.down, RaycastDistance, LayerMaskForRaycasting).collider;
                if (hitCollider == null)
                    hitCollider = Physics2D.Raycast(raycastSource, Vector2.up, RaycastDistance, LayerMaskForRaycasting).collider;

                if (hitCollider != null && hitCollider.tag == "Platform")
                {
                    _objectsPool.Peek().GetComponent<ScrollingGameObject>().ScrollSpeed = hitCollider.gameObject.GetComponent<ScrollingGameObject>().ScrollSpeed;

                    Vector3 spawnPosition =
                        _placeObjectsOnlyAtPlatformCenter
                        ?
                        hitCollider.transform.position + Vector3.up * YDistanceToPlatform
                        :
                        new Vector3(Random.Range(creatingBorderPositionX + (i / 2), hitCollider.transform.position.x + (hitCollider.bounds.size.x / 2)), hitCollider.transform.position.y + YDistanceToPlatform, _objectsPool.Peek().transform.position.z);

                    PlaceObjectFromPool(spawnPosition, gridSnapper);

                    break;
                }

            }
        }
    }
}
