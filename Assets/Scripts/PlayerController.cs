using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerPref;
    private GameObject player;
    Vector3 Position = new Vector3(0, 0.5f, 7);
    void Start()
    {
        StartCoroutine(CheckTree());

    }
    IEnumerator CheckTree()
    {
        yield return new WaitForSeconds(0.1f);
        player = Instantiate(playerPref);
        player.transform.position = Position;
        Position.Set(0, 3, 7);
        Ray ray = new Ray(Position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject tree = hit.transform.gameObject;
            if (tree.GetComponent<Deletion>())
                tree.GetComponent<GenericObject>().DiscardToPool();
        }
        CameraController.camera.SetPlayer(ref player);
        UIController.ui.SetPlayer(ref player);
    }
}
