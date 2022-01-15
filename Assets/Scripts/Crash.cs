using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crash : MonoBehaviour
{
    [SerializeField] AudioClip crash;
    private void OnTriggerEnter(Collider touch)
    {
        if (touch.GetComponent<TransportMove>() && GetComponent<Swiping>().alive == true)
        {
            GetComponent<AudioSource>().PlayOneShot(crash);
            CameraController.camera.StopMoving();
            GetComponent<Swiping>().Death();
            Vector3 sizeAfterCrash = new Vector3(0.9f, 0.2f, 0.9f);
            Vector3 PositionAfterCrash = new Vector3(transform.position.x, 0.5f, touch.transform.position.z);
            Vector3 SizeOfDeadPlayer = new Vector3(1, 0.1f, 1);
            Vector3 PositionOfDeadPlayer = new Vector3(transform.position.x, 0.5f, touch.transform.position.z);
            var Seq = DOTween.Sequence();
            transform.DOKill();
            Seq.Append(transform.DOMove(PositionAfterCrash, 0));
            Seq.Join(transform.DOScale(sizeAfterCrash, 0));
            Seq.AppendInterval(0.5f);
            Seq.Append(transform.DOScale(SizeOfDeadPlayer, 3));
            Seq.Join(transform.DOMove(PositionOfDeadPlayer, 3)).OnComplete(() => UIController.ui.Death());
        }
    }
}
