using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 2;
    public int health;
    public Transform playerPos;
    public Transform enemyPos;

    // Start is called before the first frame update
    void Start()
    {
       health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(playerPos.position, enemyPos.position);
        //Debug.Log("Distance to obj: "  + dist);
        if(Input.GetKeyDown("space"))
        {
            
            if( dist <= 1f ){
                health--;
                if(health <= 0){
                // Destroy the cube when it has no health left
                Destroy(gameObject);
                }
            }
        }
    }

    public void isHit(){
        health--;
        if(health <= 0){
            // Destroy the cube when it has no health left
            Destroy(gameObject);
        }
    }
}
