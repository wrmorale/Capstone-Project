using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 2;
    public int health;
    

    // Start is called before the first frame update
    void Start()
    {
       health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void isHit(){
        health--;
        if(health <= 0){
            // Destroy the cube when it has no health left
            Destroy(gameObject);
        }
    }
}
