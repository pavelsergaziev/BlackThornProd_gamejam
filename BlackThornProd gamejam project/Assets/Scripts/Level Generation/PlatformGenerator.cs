using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : ProximityLevelElementsGenerator
{
    private float _platformPartPrefabWidth;

    InnerGeneratorForPlatformGenerator PlatformWrappersGenerator;

    public PlatformGenerator(int platformPartsPoolSize, int platformWrappersPoolSize, Transform levelLayoutObject, GameObject platformPartPrefab, GameObject platformWrapperPrefab, int firstPlatformLength, float firstPlatformStartingPositionX, Sprite leftEdge, Sprite[] middleSprites, Sprite rightEdge, PixelGridSnapper gridSnapper) : base(platformPartsPoolSize, levelLayoutObject, platformPartPrefab)
    {
        _platformPartPrefabWidth = platformPartPrefab.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.x;
        PlatformWrappersGenerator = new InnerGeneratorForPlatformGenerator(platformWrappersPoolSize, levelLayoutObject, platformWrapperPrefab, platformPartPrefab.GetComponent<ScrollingGameObject>().ScrollSpeed);

        CreatePlatform(firstPlatformLength, new Vector3(levelLayoutObject.position.x + firstPlatformStartingPositionX, levelLayoutObject.position.y, levelLayoutObject.position.z), leftEdge, middleSprites, rightEdge, gridSnapper);
    }

    /// <summary>
    /// Попытаться создать платформу.
    /// </summary>
    /// <param name="creatingBorderPositionX"></param>
    /// <param name="minPlatformLength"></param>
    /// <param name="maxPlatformLength"></param>
    /// <param name="minXDistanceBetweenPlatforms"></param>
    /// <param name="maxXDistanceBetweenPlatforms"></param>
    /// <param name="minYDistanceBetweenPlatforms"></param>
    /// <param name="maxYDistanceBetweenPlatforms"></param>
    /// <param name="leftEdge"></param>
    /// <param name="middleSprites"></param>
    /// <param name="rightEdge"></param>
    /// <param name="gridSnapper"></param>
    public void CheckAndTryCreatePlatform(int creatingBorderPositionX, int minPlatformLength, int maxPlatformLength, float minXDistanceBetweenPlatforms, float maxXDistanceBetweenPlatforms, float minYDistanceBetweenPlatforms, float maxYDistanceBetweenPlatforms, Sprite leftEdge, Sprite[] middleSprites, Sprite rightEdge, PixelGridSnapper gridSnapper)
    {
        Transform rightmostActivePlatformPart = _activeObjects[_activeObjects.Count - 1].transform;
        if (rightmostActivePlatformPart.position.x < creatingBorderPositionX)
            CreatePlatform(Random.Range(minPlatformLength, maxPlatformLength + 1), new Vector3(rightmostActivePlatformPart.position.x + Random.Range(minXDistanceBetweenPlatforms, maxXDistanceBetweenPlatforms), rightmostActivePlatformPart.position.y + (Random.Range(minYDistanceBetweenPlatforms, maxYDistanceBetweenPlatforms) * Random.Range(-1, 2)), rightmostActivePlatformPart.transform.position.z), leftEdge, middleSprites, rightEdge, gridSnapper);
    }

    private void CreatePlatform(int platformLength, Vector3 startingPosition, Sprite leftEdge, Sprite[] middleSprites, Sprite rightEdge, PixelGridSnapper gridSnapper)
    {
        PlaceObjectFromPool(startingPosition, leftEdge, gridSnapper);

        for (int i = 0; i < platformLength - 2; i++)
            PlaceObjectFromPool(new Vector3(startingPosition.x + ((i + 1) * _platformPartPrefabWidth), startingPosition.y, startingPosition.z), middleSprites[Random.Range(0, middleSprites.Length)], gridSnapper);

        PlaceObjectFromPool(new Vector3(startingPosition.x + ((platformLength - 1) * _platformPartPrefabWidth), startingPosition.y, startingPosition.z), rightEdge, gridSnapper);
        
        PlatformWrappersGenerator.PlacePlatformFromPool((startingPosition + _activeObjects[_activeObjects.Count - 1].transform.position) / 2, new Vector2(_platformPartPrefabWidth * platformLength, PlatformWrappersGenerator.NextPlatformInPool.GetComponent<BoxCollider2D>().size.y));
    }


    public override void CheckAndTryRemoveObjects(int destroyingBorderPositionX)
    {
        base.CheckAndTryRemoveObjects(destroyingBorderPositionX);
        PlatformWrappersGenerator.CheckAndTryRemoveObjects(destroyingBorderPositionX);
    }
}
