using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObject : MonoBehaviour, IPooledObject
{
    public string poolTag;
    public bool isActive = false;

    public void OnRequestedFromPool()
    {
        isActive = true;
    }

    public void DiscardToPool()
    {
        MyPooler.ObjectPooler.Instance.ReturnToPool(poolTag, this.gameObject);
        isActive = false;
    }

    /*void Update()
    {
        if (isActive)
        {
            timeToDestroy -= Time.deltaTime;
            if (timeToDestroy <= 0f)
            {
                DiscardToPool();
            }
        }
    }*/
}
