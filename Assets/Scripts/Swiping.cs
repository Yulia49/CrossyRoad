using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Swiping : MonoBehaviour
{
    Vector3 tapPosition;
    Vector3 swipe;
    public bool alive = false;
    [SerializeField] AudioClip waterPluh;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip crash1;
    private bool isMobile;

    void Start()
    {
        alive = false;
        isMobile = Application.isMobilePlatform;
    }

    void Update()
    {
        if (transform.position.x < -4.5 || transform.position.x > 4.5)
        {
            Death();
            StartCoroutine(NoParent());
            UIController.ui.Death();
        }

        if (alive)
            if(!isMobile)
            {
                if (Input.GetMouseButtonDown(0))
                { 
                    tapPosition = Input.mousePosition;
                    GetComponent<Animator>().SetTrigger("sit");
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    swipe = Input.mousePosition - tapPosition;
                    Move();
                }
                //Debug.Log(transform.position);
            }
            else 
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        tapPosition = Input.mousePosition;
                        GetComponent<Animator>().SetTrigger("sit");
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        swipe = Input.mousePosition - tapPosition;
                        Move();
                    }
                }
            }
    }
    void Move()
    {
        float RotationY = 0;
        float PositionX = 0;
        float PositionZ = 0;
        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            RotationY = swipe.x > 0 ? 90 : -90;
            PositionX = swipe.x > 0 ? 1 : -1;
        }
        else
        { 
            RotationY = swipe.y > 0 ? 0 : 180;
            PositionZ = swipe.y > 0 ? 1 : -1;
        }
        Vector3 moving = new Vector3(PositionX, 0, PositionZ);
        Ray ray = new Ray(transform.position + Vector3.up/2, moving);
        RaycastHit hit;
        bool let = true;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < 1)
            {
                GameObject obj = hit.transform.gameObject;
                if (obj.GetComponent<TransportMove>())
                {
                    alive = false;
                    let = false;
                    GetComponent<AudioSource>().PlayOneShot(crash1);
                    transform.SetParent(obj.transform);
                    var Seq = DOTween.Sequence();
                    Vector3 m = new Vector3(0, 0, -0.55f + (1.1f * obj.GetComponent<TransportMove>().CarDirection));
                    Vector3 v = new Vector3(0.5f, 0.9f, 0.1f);
                    Seq.Append(transform.DOScale(v, 0));
                    Seq.Join(transform.DOLocalMove(m, 0)).OnComplete(() =>
                    {
                        UIController.ui.Death();
                        Death();
                    });
                }
                else if (obj.GetComponent<Forest>())
                {
                    let = false;
                    GetComponent<Animator>().SetTrigger("standUp");
                    Vector3 rotate = new Vector3(0, RotationY, 0);
                    transform.DORotate(rotate, 0.1f, RotateMode.Fast);
                }
            }
        }
        if (let)
        {

            bool check = checkForest(transform.position + moving);
            if (!check)
            {
                transform.parent = null;
                transform.DOMoveY(0.5f, 0);
                GetComponent<AudioSource>().PlayOneShot(jump);
                GetComponent<Animator>().SetTrigger("jump");
                Vector3 rotate = new Vector3(0, RotationY, 0);
                transform.DORotate(rotate, 0.1f, RotateMode.Fast);
                transform.DOMove(transform.position + moving, 0.3f).OnComplete(() => CheckDown());
                LineController.line.Moving(moving);
                CameraController.camera.Move(moving);
            }
            else
                GetComponent<Animator>().SetTrigger("standUp");
        }
        else  GetComponent<Animator>().SetTrigger("standUp");
        
    }
    public void Death()
    {
        alive = false;
        CameraController.camera.StopMoving();
    }
    IEnumerator NoParent()
    {
        yield return new WaitForSeconds(0.5f);
        transform.parent = null;
    }
    void CheckDown()
    {
        Vector3 PositionRay = new Vector3(transform.position.x, 1, transform.position.z);
        Ray ray = new Ray(PositionRay, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject down = hit.transform.gameObject;
            if (down.GetComponent<WaterController>())
            {
                CameraController.camera.StopMoving();
                GetComponent<AudioSource>().PlayOneShot(waterPluh);
                Death();
                var Seq = DOTween.Sequence();
                Seq.Append(transform.DOMoveZ(down.transform.position.z, 0));
                Seq.Append(transform.DOMoveY(-5, 0.5f)).OnComplete(() => UIController.ui.Death());
            }
            if (down.GetComponent<WaterObject>())
            {
                transform.SetParent(down.transform);
                var Seq = DOTween.Sequence();
                Seq.Append(down.transform.DOLocalMoveY(down.transform.position.y - 0.1f, 0.5f));
                Seq.Append(down.transform.DOLocalMoveY(down.transform.position.y, 0.5f));
            }
        }
    }
    bool checkForest(Vector3 CheckPosition)
    {
        bool check = false;
        if (CheckPosition.x < -4 || CheckPosition.x > 4.5)
            check = true;
        return check;

    }
}
