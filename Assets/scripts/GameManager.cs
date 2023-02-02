using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

//this will keep track of stats for player across scenes. Also helpful for testing since stats can be changed with sliders
[System.Serializable]
public class playerStats{
    public float maxHealth;
    public float movementSpeed;
    public float basicDamage;
    public float attackSpeed;
    public float cooldownReduction;
}
//this class can be used by both the player and enemies
[System.Serializable]
public class Ability{
    public string abilityName = "";
    public string abilityType = ""; //this would be like aoe, single target, heal, etc
    public float abilityDamage = 1.0; //negative # should work for healing
    public float abilityRange = 1.0;
    public float abilityCooldown = 1.0;
    public float castTime = 1.0;
    public float damageMultiplier = 1.0;
    //public Transform abilityAnimation; //not tested but should work to play correct animation
}

public class GameManager : MonoBehaviour{
    public static GameManager instance;

    public float timer;
    public Text timerText;
    public bool roomCleared;
    public int currentGold; 
    public List<String> availableAbilities = new List<String>(); //not sure how we will keep track of abilities yet but a list of strings to hold ablities that can be learned
    
    //setup singleton
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        timer = 0;
        roomCleared = false;
        currentGold = 0;
    }

    void Update(){
        timer += Time.deltaTime;
        //Debug.Log("Time: " + timer.ToString("F2")); //timer displays in console for now

        //Checks to see if enemies are still in arena
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if(enemies.Length == 0){
            roomCleared = true; 
            //Add some code to advance to next scene
        }
    }
}
