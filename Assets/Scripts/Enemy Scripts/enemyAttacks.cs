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
    float longestAttackRange = 0;

    void Start(){
        enemyInstance.movement = GetComponent<enemyMovement>();
        //this just gets longest range to see when the enemy can start to cast abilities or attacking the player
        foreach (Ability ability in enemyInstance.abilities) {
            if(longestAttackRange < ability.abilityRange){
                longestAttackRange = ability.abilityRange;
            }
        }
        if(longestAttackRange < enemyInstance.attackRange){
            longestAttackRange = enemyInstance.attackRange;
        }
    }

    void Update(){
        //if able to attack, the enemy does so
        //first checks to see if enemy is in range for an attack
        if(Vector2.Distance(enemyInstance.body.position, enemyInstance.playerBody.position) < longestAttackRange && actionCooldownTimer <= 0) {
            enemyAction();
        }
        actionCooldownTimer -= Time.deltaTime;
        abilityCooldownTimer -= Time.deltaTime;
        if(abilityCooldownTimer < 0) {
            abilityCooldownTimer = 0;
        } 
    }

    // function to control enemy actions 
    private void enemyAction(){
        //if off ability cooldown can use ability depending on chance to use that ability
        if(abilityCooldownTimer == 0){
            counter = 0;
            foreach (Ability ability in enemyInstance.abilities) {
                //before checking if an ability can be cast check if the player is in ability range
                if(Vector2.Distance(enemyInstance.body.position, enemyInstance.playerBody.position) < ability.abilityRange){
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < ability.abilityChance) {
                        useAbility(counter);
                        actionCooldownTimer = (1 / enemyInstance.basicAttackSpeed);
                        break;
                    }
                    counter++;
                }
            }
        }
        enemyInstance.usingAbility = false;
        //if ability not used will attack if in range
        if(Vector2.Distance(enemyInstance.body.position, enemyInstance.playerBody.position) < enemyInstance.attackRange){
            attack(); //basic attack
            actionCooldownTimer = (1 / enemyInstance.basicAttackSpeed);
        }
        
    }

    private void attack(){
        //play basic attack animation of this enemy - for now something generic for our enemy (makes enemy jump)
        //the attack animation should be checking for collisions so it should do damage that way. 
        checkCollision(enemyInstance.basicAttackDamage); //temp damage dealing
    }

    private void useAbility(int abilityNum){
        enemyInstance.usingAbility = true;
        //play ability animation for ability used - for now something generic for our enemy (makes enemy jump)
        //enemyInstance.body.AddForce(Vector3.up * 4, ForceMode.Impulse);
        checkCollision(enemyInstance.abilities[abilityNum].abilityDamage);
        abilityCooldownTimer = enemyInstance.abilities[abilityNum].abilityCooldown;

        //this is what the abilities should really do=
        //first check what type of ability it is and will do stuff depending on type of ability
        if(enemyInstance.abilities[abilityNum].abilityType == "Movement"){
            //should play animation here
            //since this ability is for movement here is some generic code that will make it move 
            enemyInstance.movement.abilityMovement();
            
        }
        else if(enemyInstance.abilities[abilityNum].abilityType == "AoE"){
            //again play animation here
            //pretty generic code but will deal damage to player if around certain radius during animation frames
            
        }
        //have other ability types as else if statments and we can add simple code to deal damage correctly. 
    }

    private void checkCollision(float damage){ //for now just checks for collisions to deal damage. Will probably change once hitboxes and animations are in for enemies
        //checks to see if "attack" collides with player
        Collider[] colliders = Physics.OverlapSphere(enemyInstance.body.position, enemyInstance.attackRange);
        foreach (Collider collider in colliders) {
            if (collider.tag == "Player") {
                // apply damage to player
                Player player = collider.GetComponent<Player>();
                player.isHit(damage);
            }
        }
    }
}
