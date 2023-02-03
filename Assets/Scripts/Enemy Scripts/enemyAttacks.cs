using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttacks : MonoBehaviour
{
    public Enemy enemyInstance;
    public float basicAttackCooldownTimer = 0;
    public float abilityCooldownTimer = 0;
    public float actionCooldownTimer = 0;
    public int counter = 0;

    void Update(){
        //if able to attack, the enemy does so
        if(Vector2.Distance(enemyInstance.body.position, enemyInstance.playerBody.position) < enemyInstance.attackRange && actionCooldownTimer <= 0) {
            enemyAction();
        }
        actionCooldownTimer -= Time.deltaTime;
        abilityCooldownTimer -= Time.deltaTime;
    }

    // function to control enemy actions 
    private void enemyAction(){
        //if off ability cooldown can use ability depending on chance to use that ability
        if(abilityCooldownTimer == 0){
            counter = 0;
            foreach (Ability ability in enemyInstance.abilities) {
                float randomNumber = Random.Range(0, 100);
                if (randomNumber < ability.abilityChance) {
                    useAbility(counter);
                    break;
                }
                counter++;
            }
        }
        //if not will just basic attack
        else{
            attack();
        }
        actionCooldownTimer = (1 / enemyInstance.basicAttackSpeed);
    }

    private void attack(){
        //play attack animation - for now something generic for our cube enemy (makes enemy jump)
        enemyInstance.body.AddForce(Vector3.up * 1, ForceMode.Impulse);
        //checks to see if "attack" collides with player
        Collider[] colliders = Physics.OverlapSphere(enemyInstance.body.position, enemyInstance.attackRange);
        foreach (Collider collider in colliders) {
            if (collider.tag == "Player") {
                // apply damage to player
                Player player = collider.GetComponent<Player>();
                player.isHit(enemyInstance.basicAttackDamage);
            }
        }
    }

    private void useAbility(int abilityNum){
        Debug.Log("Ability " + enemyInstance.abilities[abilityNum].abilityName + " has been cast.");
        abilityCooldownTimer = enemyInstance.abilities[abilityNum].abilityCooldown;
        actionCooldownTimer = (1 / enemyInstance.basicAttackSpeed);
    }
}
