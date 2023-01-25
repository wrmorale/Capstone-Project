using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection : MonoBehaviour
{
    public playerController pc;
    public Enemy enemy;

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Enemy" && pc.isAttacking){
            enemy.isHit();
        }
    }
}
