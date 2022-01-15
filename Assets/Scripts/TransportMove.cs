using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransportMove : MonoBehaviour
{
    public float CarDirection;
    public void Moving(float speed, float direction)
    {
        CarDirection = direction;
        transform.DOMoveX(-transform.position.x, 10 - speed).SetEase(Ease.Linear).OnComplete(() => GetComponent<GenericObject>().DiscardToPool());
    }
}
