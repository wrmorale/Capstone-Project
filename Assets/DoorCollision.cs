using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollision : MonoBehaviour
{

    public void OnTriggerEnter(Collider player) {
        // TODO: make popup appear here
        transform.parent.GetComponent<GameManager>().isNextToExit = true;
        Debug.Log("ready to exit");
    }
}
