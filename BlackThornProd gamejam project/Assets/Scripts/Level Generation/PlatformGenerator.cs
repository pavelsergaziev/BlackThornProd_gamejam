﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : BaseLevelElementsGenerator
{
    private float _platformPartPrefabWidth;

    public PlatformGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, int firstPlatformLength, Sprite leftEdge, Sprite[] middleSprites, Sprite rightEdge, PixelGridSnapper gridSnapper) : base(objectsPoolSize, levelLayoutObject, objectPrefab)
    {
        _platformPartPrefabWidth = objectPrefab.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.x;
        CreatePlatform(firstPlatformLength, levelLayoutObject.position, leftEdge, middleSprites, rightEdge, gridSnapper);
    }

    public void CheckAndTryCreateObject(int creatingBorderPositionX, int minPlatformLength, int maxPlatformLength, float minXDistanceBetweenPlatforms, float maxXDistanceBetweenPlatforms, float minYDistanceBetweenPlatforms, float maxYDistanceBetweenPlatforms, Sprite leftEdge, Sprite[] middleSprites, Sprite rightEdge, PixelGridSnapper gridSnapper)
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
        
    }
}