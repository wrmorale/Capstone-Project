using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("stats")]
    [SerializeField] private float maxHealth;
    public float health;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxHealth;

    
    void Start(){
       health = maxHealth;
    }

    void Update(){

    }

    public void isHit(){
        health-=1.0;
        if(health <= 0){
            // Destroy the cube when it has no health left
            Destroy(gameObject);
        }
    }
}
