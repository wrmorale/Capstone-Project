using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttacks : MonoBehaviour
{
    [SerializeField] private Transform player;
    public Enemy enemyInstance;

    void Start(){
        print(enemyInstance.basicAttackDamage);
    }

    void Update(){
        
    }
}
