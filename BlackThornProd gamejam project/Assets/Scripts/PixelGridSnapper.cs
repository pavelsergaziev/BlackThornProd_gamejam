using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGridSnapper {

    private int _ppu;
    private int _ppt;
    private int _screenPixelGridIncrement;

    public PixelGridSnapper(int pixelsPerUnit, int pixelsPerTexel)
    {
        _ppu = pixelsPerUnit;
        _ppt = pixelsPerTexel;

        _screenPixelGridIncrement = _ppu * _ppt;
    }

    public void SnapToTexelGrid(Transform childTransformToSnap, Transform parentTransform)
    {
        SnapToGrid(childTransformToSnap, parentTransform, _ppu);
        parentTransform.position = childTransformToSnap.position;
    }

    public void SnapToScreenPixelGrid(Transform childTransformToSnap, Transform parentTransform)
    {
        SnapToGrid(childTransformToSnap, parentTransform, _screenPixelGridIncrement);
    }



    private void SnapToGrid(Transform childTransformToSnap, Transform parentTransform, int gridIncrement)
    {
        childTransformToSnap.localPosition = new Vector3
                                                 (
                                                    (Mathf.Round(parentTransform.position.x * gridIncrement) / gridIncrement) - parentTransform.position.x,
                                                    (Mathf.Round(parentTransform.position.y * gridIncrement) / gridIncrement) - parentTransform.position.y,
                                                    childTransformToSnap.localPosition.z
                                                 );
    }

}
