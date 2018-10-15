using UnityEngine;
/// <summary>
/// Должен находиться на обьекте, который можно подобрать, либо схватить гарпуном
/// </summary>
[RequireComponent(typeof(Collider2D))]
class PickUp:MonoBehaviour
{
    public PickUps TypeOf;
}

