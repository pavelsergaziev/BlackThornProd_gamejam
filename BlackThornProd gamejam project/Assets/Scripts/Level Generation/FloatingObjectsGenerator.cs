using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Генератор объектов, "парящих в воздухе".
/// Имеется в виду, что это объекты, которые не лежат на платформах, а находятся над ними, и с ними можно как-то взаимодействовать, например, стрелять по ним.
/// </summary>
public class FloatingObjectsGenerator : BaseLevelElementsGenerator {
    public FloatingObjectsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab) : base(objectsPoolSize, levelLayoutObject, objectPrefab)
    {
    }
    
}
