using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deletion : MonoBehaviour
{
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
        {
            GetComponent<GenericObject>().DiscardToPool();
        }
    }
}
