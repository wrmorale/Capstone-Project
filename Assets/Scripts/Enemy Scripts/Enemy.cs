using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("stats")]
    [SerializeField]private string enemyName;
    [SerializeField]private double maxHealth;
    [SerializeField]public double health;
    [SerializeField]public float basicAttackDamage;
    [SerializeField]private float movementSpeed;
    public List<Ability> abilities; 

    
    void Start(){
       health = maxHealth;
    }

    void Update(){

    }

    public void isHit(){
        health-=1.0f;
        if(health <= 0){
            // Destroy the cube when it has no health left
            Destroy(gameObject);
        }
    }
}
