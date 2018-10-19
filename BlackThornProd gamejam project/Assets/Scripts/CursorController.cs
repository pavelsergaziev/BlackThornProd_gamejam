using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    bool _customCursorIsOn;
    private Camera _camera;

    private void Start()
    {
        _customCursorIsOn = false;
        _camera = FindObjectOfType<Camera>();
        SwitchToNormalCursor();
    }
    private void Update()
    {
        if (_customCursorIsOn)
        {
            transform.position = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void SwitchToCustomCursor()
    {
        gameObject.SetActive(true);
        Cursor.visible = false;
        
        _customCursorIsOn = true;
    }
    public void SwitchToNormalCursor()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
        _customCursorIsOn = false;
    }
}
