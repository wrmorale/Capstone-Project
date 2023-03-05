using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;

public class collisionDetection : MonoBehaviour
{
    public playerController pc;
    public Player player;
    public Enemy enemy;
    public DustPile dustPile;
    public Furniture furniture;
    
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Enemy" && pc.state == States.PlayerStates.Attacking){
            //applies to all with the enemy tag
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.isHit(player.basicDamage);
        }

        if(other.tag == "DustPile" && pc.state == States.PlayerStates.Attacking){
            DustPile dustPile = other.GetComponent<DustPile>();
            dustPile.isHit(player.basicDamage);
        }

        if(other.tag == "Furniture" && pc.state == States.PlayerStates.Attacking){
            Furniture furniture = other.GetComponent<Furniture>();
            furniture.isHit(player.basicDamage);
        }
    }
}
