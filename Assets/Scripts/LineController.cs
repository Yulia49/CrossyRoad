using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    float z, typeOfLine, countOfSimilar, countOfGrass;
    Queue<GameObject> Lines = new Queue<GameObject>();
    bool PrevLily = false;

    public static LineController line;
    GameObject obj;

    void Awake()
    {
        if (line == null)
            line = this;
        for (; z <10; z++)
            AddLine();
        for (; z < 40;)
        {
            typeOfLine = Random.Range(0, 5);
            countOfSimilar = Random.Range(1, 5);
            for (float i = countOfSimilar; i > 0; i--)
            {
                AddLine();
                z++;
            }
        }
    }
    void Update()
    {
        
    }
    void AddLine()
    {
        Vector3 LinePosition = new Vector3(0, 0, z);
        switch(typeOfLine)
        {
            case 0:
            case 1:
                {
                    countOfGrass++;
                    obj = (countOfGrass % 2 == 1)? MyPooler.ObjectPooler.Instance.GetFromPool("Grass1", LinePosition, Quaternion.identity) :
                        MyPooler.ObjectPooler.Instance.GetFromPool("Grass2", LinePosition, Quaternion.identity);
                    Lines.Enqueue(obj);
                    obj.GetComponent<GrassController>().CreateForest();
                    StartCoroutine(CreateCoin());
                    break;
                }
            case 2:
            case 3:
                {
                    countOfGrass = 0;
                    Lines.Enqueue(obj = MyPooler.ObjectPooler.Instance.GetFromPool("Road", LinePosition, Quaternion.identity));
                    obj.GetComponent<RoadController>().CarMove();
                    StartCoroutine(CreateCoin());
                    break;
                }
            default:
                {
                    countOfGrass = 0;
                    Lines.Enqueue(obj = MyPooler.ObjectPooler.Instance.GetFromPool("Water", LinePosition, Quaternion.identity));
                    obj.GetComponent<WaterController>().LogCreate(ref PrevLily);
                    break;
                }
        }
    }
    public void Moving(Vector3 move)
    {
        if (move.z > 0)
        {
            if (countOfSimilar == 0)
            {
                typeOfLine = Random.Range(0, 5);
                countOfSimilar = Random.Range(1, 7);
            }
            UIController.ui.MoveScore();
            AddLine();
            obj = Lines.Dequeue();
            obj.GetComponent<GenericObject>().DiscardToPool();
            countOfSimilar--;
            z++;
        }
    }
    IEnumerator CreateCoin()
    {
        float chance = Random.Range(0, 5);
        float PositionX;
        Vector3 Position = new Vector3();
        if (chance == 0)
        {
            PositionX = Random.Range(-4, 5);
            Position.Set(PositionX, 0.55f, z);
            yield return new WaitForSeconds(0.1f);
            CheckTree(Position);
            MyPooler.ObjectPooler.Instance.GetFromPool("Coin", Position, Quaternion.identity);
        }
    }
    void CheckTree(Vector3 Position)
    {
        Ray ray = new Ray(Position + 3*Vector3.up, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject tree = hit.transform.gameObject;
            if (tree.GetComponent<Forest>())
                tree.GetComponent<GenericObject>().DiscardToPool();
        }
    }
}
