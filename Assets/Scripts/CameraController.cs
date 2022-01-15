using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController camera;
    public bool alive = false;
    [SerializeField] private GameObject birdPref;
    private GameObject player, bird;
    float speed;

    void Awake()
    {
        if (camera == null)
            camera = this;
    }
    void Update()
    {
        if (alive)
        {
            speed = -(player.transform.position.z - transform.position.z) + 6;
            transform.DOMoveZ(transform.position.z + 1, speed);
            transform.DOMoveX(player.transform.position.x + 0.5f, 0);
            if (player.transform.position.z - transform.position.z < 1)
            {
                StartCoroutine(KillerBird());

            }
        }
    }
    public void Move(Vector3 moving)
    {
        transform.DOMove(transform.position + moving, 0.5f);

    }
    public void StopMoving()
    {
        transform.DOKill();
        player.GetComponent<Swiping>().alive = false;
        alive = false;
    }
    IEnumerator KillerBird()
    {
        StopMoving();
        bird = Instantiate(birdPref);
        Vector3 birdPosition = new Vector3(player.transform.position.x, 3, player.transform.position.z + 11);
        bird.transform.position = birdPosition;
        var Seq = DOTween.Sequence();
        Seq.Append(transform.DOMoveZ(player.transform.position.z - 3, 0));
        Seq.Append(bird.transform.DOMoveZ(player.transform.position.z - 5, 3));
        yield return new WaitForSeconds(1.5f);
        player.transform.DOMoveY(-5, 0).OnComplete(() => UIController.ui.Death());
    }
    public void SetPlayer(ref GameObject playerOrig)
    {
        player = playerOrig;
    }
}
