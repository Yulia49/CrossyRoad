using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterController : MonoBehaviour
{
    float type, size, direction, speed, chance;
    bool SomethingCreate = false;
    Vector3 Position = new Vector3();
    Vector3 LilyRotation = new Vector3(0, 45, 0);

    public void LogCreate(ref bool PrevLily)
    {
        type = Random.Range(0, 4);
        if (type == 0 && !PrevLily)
        {
            SomethingCreate = true;
            PrevLily = true;
            CreateLily();
        }
        else
        {
            SomethingCreate = true;
            direction = Random.Range(0, 2);
            speed = Random.Range(3, 7);
            StartCoroutine(GoLog());
        }
        if (!SomethingCreate)
            LogCreate(ref PrevLily);
    }
    IEnumerator GoLog()
    {
        size = Random.Range(0, 3);
        Position.Set(13 - 26 * direction, 0.6f, transform.position.z);
        switch (size)
        {
            case 0:
                {
                    GameObject obj = MyPooler.ObjectPooler.Instance.GetFromPool("Log1", Position, Quaternion.identity);
                    obj.GetComponent<LogMove>().Moving(speed);
                    break;
                }
            case 1:
                {
                    GameObject obj = MyPooler.ObjectPooler.Instance.GetFromPool("Log2", Position, Quaternion.identity);
                    obj.GetComponent<LogMove>().Moving(speed);
                    break;
                }
            default:
                {
                    GameObject obj = MyPooler.ObjectPooler.Instance.GetFromPool("Log3", Position, Quaternion.identity);
                    obj.GetComponent<LogMove>().Moving(speed);
                    break;
                }
        }
        yield return new WaitForSeconds(- speed + 8);
        StartCoroutine(GoLog());
    }
    public void CreateLily()
    {
        bool create = false;
        for (float x = -4f; x < 5f; x++)
        {
            chance = Random.Range(0, 6);
            if (chance == 0)
            {
                create = true;
                Position.Set(x, 0.6f, transform.position.z);
                GameObject obj = MyPooler.ObjectPooler.Instance.GetFromPool("Lily", Position, Quaternion.identity);
                obj.transform.DORotate(LilyRotation, 0);
            }         
        }
        if (!create)
            CreateLily();
    }
}
