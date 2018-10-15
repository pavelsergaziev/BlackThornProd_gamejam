using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Генератор объектов, которые непосредственно лежат на платформах.
/// </summary>
public class ObjectsLyingOnPlatformsGenerator : TimedLevelElementsGenerator
{
    
    private const int AttemptsToPlaceObject = 10;//просто какой-то предел, чтобы в бесконечный цикл не попасть

    public ObjectsLyingOnPlatformsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, int delayBeforeFirstPlacement, int minDelayBetweenObjectPlacements, int maxDelayBetweenObjectPlacements) : base(objectsPoolSize, levelLayoutObject, objectPrefab, delayBeforeFirstPlacement, minDelayBetweenObjectPlacements, maxDelayBetweenObjectPlacements)
    {
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

                RaycastHit2D hit = Physics2D.Raycast(raycastSource, Vector2.down);
                if (!hit)
                    hit = Physics2D.Raycast(raycastSource, Vector2.up);

                if (hit && hit.collider.tag == "Platform")
                {
                    PlaceObjectFromPool(hit.collider.transform.position + Vector3.up * YDistanceToPlatform, gridSnapper);
                    break;
                }

            }
        }
    }
}
