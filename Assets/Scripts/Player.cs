using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //this will keep track of stats for player
    [Header("stats")]
    [SerializeField]public float maxHealth;
    [SerializeField]public float movementSpeed;
    [SerializeField]public float basicDamage;
    [SerializeField]public float attackSpeed;
    [SerializeField]public float cooldownReduction;
    public bool alive;
    public int lives;
    public bool isInvulnerable;
    [SerializeField]public float health;
    public List<Ability> abilities; 
    public Transform platform;
    public float fallLimit = -10; 

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        alive = true;
        isInvulnerable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < platform.position.y + fallLimit){
            health = 0;
            alive = false;
        }
    }

    public void isHit(float damage){
        if (!isInvulnerable){
            print("Player took " + damage + " damage");
            health -= damage;
            if(health <= 0){
                alive = false;
            }
        }
        else 
            Debug.Log("isInvulnerable");
    }
}
