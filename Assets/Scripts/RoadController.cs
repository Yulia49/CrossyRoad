using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadController : MonoBehaviour
{
    float typeF, direction, speed, interval;
    string type;
    Vector3 Position = new Vector3();
    Quaternion rotation = new Quaternion();
    Vector3 RailsPosition = new Vector3();
    Quaternion Railsrotation = new Quaternion(0, 0, 0, 1);
    [SerializeField] AudioClip railway;
    public void CarMove()
    {
        typeF = Random.Range(0, 5);
        direction = Random.Range(0, 2);
        speed = Random.Range(1, 7);
        Position.Set(13 - 26*direction, 0.5f, transform.position.z);
        rotation.Set(0f, 180 * direction, 0f, 1);
        switch(typeF)
        {
            case 0:
                {
                    type = "Car1";
                    StartCoroutine(Cars());
                    break;
                }
            case 1:
                {
                    type = "Car2";
                    StartCoroutine(Cars());
                    break;
                }
            case 2:
                {
                    type = "Car3";
                    StartCoroutine(Cars());
                    break;
                }
            case 3:
                {
                    type = "Car4";
                    StartCoroutine(Cars());
                    break;
                }
            default:
                {
                    type = "Train";
                    RailsPosition.Set(0, 0.51f, transform.position.z);
                    interval = Random.Range(0, 10);
                    speed = 9.5f;
                    GameObject obj = MyPooler.ObjectPooler.Instance.GetFromPool("Rails", RailsPosition, Railsrotation);
                    StartCoroutine(WaitTrain());
                    break;
                }
        }

    }
    IEnumerator Cars()
    {
        GameObject obj = MyPooler.ObjectPooler.Instance.GetFromPool(type, Position, rotation);
        obj.GetComponent<TransportMove>().Moving(speed, direction);
        yield return new WaitForSeconds(-speed + 10);
        StartCoroutine(Cars());
    }
    IEnumerator Trains()
    {
        GameObject light;
        Vector3 LightPosition = new Vector3(-0.5f, 1.7f , transform.position.z - 0.6f);
        light = MyPooler.ObjectPooler.Instance.GetFromPool("Lights", LightPosition, Quaternion.identity);
        GetComponent<AudioSource>().PlayOneShot(railway);
        yield return new WaitForSeconds(1);
        light.GetComponent<GenericObject>().DiscardToPool();
        GameObject obj = MyPooler.ObjectPooler.Instance.GetFromPool(type, Position, rotation);
        obj.GetComponent<TransportMove>().Moving(speed, direction);
        StartCoroutine(WaitTrain());
    }
    IEnumerator WaitTrain()
    {
        yield return new WaitForSeconds(interval);
        interval = 10;
        StartCoroutine(Trains());
    }
}
