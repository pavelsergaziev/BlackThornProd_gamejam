using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Генератор объектов, "парящих в воздухе".
/// Имеется в виду, что это объекты, которые не лежат на платформах, а находятся над ними, и с ними можно как-то взаимодействовать, например, стрелять по ним.
/// </summary>
public class FloatingObjectsGenerator : TimedLevelElementsGenerator
{

    private const int AttemptsToFindNearestPlatform = 10;//просто какой-то предел, чтобы в бесконечный цикл не попасть

    public FloatingObjectsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, int delayBeforeFirstPlacement, int minDelayBetweenObjectPlacements, int maxDelayBetweenObjectPlacements) : base(objectsPoolSize, levelLayoutObject, objectPrefab, delayBeforeFirstPlacement, minDelayBetweenObjectPlacements, maxDelayBetweenObjectPlacements)
    {
    }

    public void CheckAndTryCreateFloatingObject(int creatingBorderPositionX, float minHeightAbovePlatform, float maxHeightAbovePlatform, PixelGridSnapper gridSnapper)
    {
        if (CheckIsItTimeToPlaceObject())
        {
            for (int i = 0; i < AttemptsToFindNearestPlatform; i++)
            {

                Vector2 raycastSource = new Vector2(creatingBorderPositionX - (i / 2), 0);

                RaycastHit2D hit = Physics2D.Raycast(raycastSource, Vector2.down, RaycastDistance, LayerMaskForRaycasting);
                if (!hit)
                    hit = Physics2D.Raycast(raycastSource, Vector2.up, RaycastDistance, LayerMaskForRaycasting);

                if (hit && hit.collider.tag == "Platform")
                {
                    //добавил немного рандома в положение по иксу
                    PlaceObjectFromPool(new Vector3(creatingBorderPositionX + Random.Range(0,2f), hit.collider.transform.position.y + Random.Range(minHeightAbovePlatform, maxHeightAbovePlatform), _objectsPool.Peek().transform.position.z), gridSnapper);
                    break;

                    
                }
            }

        }
    }
}
