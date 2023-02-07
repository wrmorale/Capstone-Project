using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;

public class collisionDetection : MonoBehaviour
{
    public playerController pc;
    public Player player;
    public Enemy enemy;
    
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Enemy" && pc.state == States.PlayerStates.Attacking){
            //applies to all with the enemy tag
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.isHit(player.basicDamage);
        }
    }
}
