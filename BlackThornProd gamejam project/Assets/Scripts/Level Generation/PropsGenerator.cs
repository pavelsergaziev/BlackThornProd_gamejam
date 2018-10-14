using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsGenerator : BaseLevelElementsGenerator {

    public PropsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, Vector3 firstObjectPosition, PixelGridSnapper gridSnapper) : base(objectsPoolSize, levelLayoutObject, objectPrefab)
    {
        PlaceFirstObject(firstObjectPosition, gridSnapper);
    }
    
}
