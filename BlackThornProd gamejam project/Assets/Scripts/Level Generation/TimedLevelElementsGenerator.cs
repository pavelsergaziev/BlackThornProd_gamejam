using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedLevelElementsGenerator : BaseLevelElementsGenerator
{

    private int _levelGenerationLoopCyclesToNextObjectPlacement;
    private int _minDelayBetweenObjectPlacements;
    private int _maxDelayBetweenObjectPlacements;


    public TimedLevelElementsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, int delayBeforeFirstPlacement, int minDelayBetweenObjectPlacements, int maxDelayBetweenObjectPlacements) : base(objectsPoolSize, levelLayoutObject, objectPrefab)
    {
        setStartValues(minDelayBetweenObjectPlacements, maxDelayBetweenObjectPlacements, delayBeforeFirstPlacement);
    }

    public TimedLevelElementsGenerator(int objectsPoolSize, Transform levelLayoutObject, GameObject objectPrefab, int delayBeforeFirstPlacement, int minDelayBetweenObjectPlacements, int maxDelayBetweenObjectPlacements, float objectScrollSpeed) : base(objectsPoolSize, levelLayoutObject, objectPrefab, objectScrollSpeed)
    {
        setStartValues(minDelayBetweenObjectPlacements, maxDelayBetweenObjectPlacements, delayBeforeFirstPlacement);
    }

    private void setStartValues(int minDelayBetweenObjectPlacements, int maxDelayBetweenObjectPlacements, int delayBeforeFirstPlacement)
    {
        _minDelayBetweenObjectPlacements = minDelayBetweenObjectPlacements;
        _maxDelayBetweenObjectPlacements = maxDelayBetweenObjectPlacements;
        _levelGenerationLoopCyclesToNextObjectPlacement = delayBeforeFirstPlacement;
    }

    protected bool CheckIsItTimeToPlaceObject()
    {
        if (_levelGenerationLoopCyclesToNextObjectPlacement > 0)
        {
            _levelGenerationLoopCyclesToNextObjectPlacement--;
            return false;
        }
        else
        {
            _levelGenerationLoopCyclesToNextObjectPlacement = Random.Range(_minDelayBetweenObjectPlacements, _maxDelayBetweenObjectPlacements + 1);
            return true;
        }
    }
}
