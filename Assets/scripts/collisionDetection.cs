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
            Enemy enemyEx = other.GetComponent<Enemy>();
            enemyEx.isHit(player.basicDamage);
        }

        if(other.tag == "DustPile" && pc.state == States.PlayerStates.Attacking){
            DustPile dustPile = other.GetComponent<DustPile>();
            dustPile.isHit(player.basicDamage);
        }

        if(other.tag == "Furniture" && pc.state == States.PlayerStates.Attacking){
            Furniture furniture = other.GetComponent<Furniture>();
            furniture.isHit(player.basicDamage);
        }

        if(other.tag == "Player" && this.tag != "weapon"){
            Player playerEx = other.GetComponent<Player>();
            //Debug.Log(enemy.basicAttackDamage);
            playerEx.isHit(enemy.basicAttackDamage);
        }
    }
}
