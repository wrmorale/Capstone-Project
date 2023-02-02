using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("stats")]
    [SerializeField] private double maxHealth;
    public double health;
    [SerializeField] private float movementSpeed;

    
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
