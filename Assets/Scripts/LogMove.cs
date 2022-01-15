using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogMove : MonoBehaviour
{
    public void Moving(float speed)
    {
        transform.DOMoveX(-transform.position.x, 10 - speed).SetEase(Ease.Linear).OnComplete(() => GetComponent<GenericObject>().DiscardToPool());
    }
}
