using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(+100)]
public class CameraController : BaseGameObject
{
    //ВРЕМЕННЫЙ ТЕСТОВЫЙ КАМЕРАКОНТРОЛЛЕР
    //может, он тут вообще не нужен - прилепить камеру как чайлд player-spriterenderer и всё

    public Vector3 offset;
    public float smoothSpeed;

    private Transform _player;

    private float _cameraPositionZ;
    private float _cameraPositionX;


    protected override void Start()
    {
        base.Start();

        _player = FindObjectOfType<PlayerController>().transform;
        _cameraPositionX = transform.position.x;
        _cameraPositionZ = transform.position.z;
        transform.position = new Vector3(_cameraPositionX, _player.position.y, _cameraPositionZ) + offset; ;
    }
    private void FixedUpdate()
    {
        FollowPlayer();
    }
    private void FollowPlayer()
    {
        Vector3 desiredPos = new Vector3(_cameraPositionX, _player.position.y, _cameraPositionZ) + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;
    }
}