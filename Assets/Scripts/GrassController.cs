using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrassController : MonoBehaviour
{
    float chance, type, x;


    public void CreateForest()
    {
        for (int k = 0; k < 3; k++)
        {
            for (x = -13f + 9*k; x < -4f + 9*k; x += 1f)
            {
                chance = Random.Range(0, 7);
                if ((chance > 1 && k!= 1) || (chance == 0 && k == 1))
                    CreateTree();
            }
        }
    }

    void CreateTree()
    {
        type = Random.Range(0, 5);
        Vector3 TreePosition = new Vector3(x, 0.75f, transform.position.z);
        switch(type)
        {
            case 0:
                {
                    MyPooler.ObjectPooler.Instance.GetFromPool("Tree1", TreePosition, Quaternion.identity);
                    break;
                }
            case 1:
                {
                    MyPooler.ObjectPooler.Instance.GetFromPool("Tree2", TreePosition, Quaternion.identity);
                    break;
                }
            case 2:
                {
                    MyPooler.ObjectPooler.Instance.GetFromPool("Tree3", TreePosition, Quaternion.identity);
                    break;
                }               
            case 3:
                {
                    MyPooler.ObjectPooler.Instance.GetFromPool("Tree4", TreePosition, Quaternion.identity);
                    break;
                }
            default:
                {
                    float RotationY = Random.Range(0, 4);
                    Vector3 StoneRotation = new Vector3(0, 90 * RotationY, 0);
                    GameObject obj =  MyPooler.ObjectPooler.Instance.GetFromPool("Stone", TreePosition, Quaternion.identity);
                    obj.transform.DORotate(StoneRotation, 0);
                    break;
                }
        }
    }
}
