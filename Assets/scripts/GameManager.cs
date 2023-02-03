using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


//this class can be used by both the player and enemies
[System.Serializable]
public class Ability{
    public string abilityName = "";
    public string abilityType = ""; //this would be like aoe, single target, heal, etc
    public float abilityDamage = 1; //negative # should work for healing
    public float abilityRange = 1;
    public float abilityCooldown = 1;
    public float abilityChance = 1; 
    public float castTime = 1;
    public float damageMultiplier = 1;
    //public Transform abilityAnimation; //not tested but should work to play correct animation
}

public class GameManager : MonoBehaviour{
    public static GameManager instance;

    public float timer;
    public Text timerText;
    public bool roomCleared;
    public int currentGold; 
    public List<String> availableAbilities = new List<String>(); //not sure how we will keep track of abilities yet but a list of strings to hold ablities that can be learned
    //
    public int numberOfEnemmies = 10;
    public GameObject enemyPrefab;
    public GameObject player;
    public Player playerStats;
    public int spawnSpread = 10;//how far apart the enemies spawn from each other

    //setup singleton
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        timer = 0;
        roomCleared = false;
        currentGold = 0;

        //create enemy copies at a location near the player
        Vector3 playerPos = player.transform.position;
        for(int i = 0; i < numberOfEnemmies; i++){
            GameObject enemy = Instantiate(enemyPrefab, playerPos + new Vector3(UnityEngine.Random.Range(-spawnSpread, spawnSpread), 0, UnityEngine.Random.Range(-5, 5)), Quaternion.identity);
        }
    }

    void Update(){
        timer += Time.deltaTime;
        //Debug.Log("Time: " + timer.ToString("F2")); //timer displays in console for now

        //Checks to see if enemies are still in arena
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        //deletes the enemy from the array if it has been destroyed
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                Array.Clear(enemies, i, 1);
            }
        }
        if(!playerStats.alive){
            Debug.Log("You're Dead, Loser");
            //here we could insert a scene jump to a losing scene
        }
        if(enemies.Length == 0){
            roomCleared = true; 
            //Room clear condition successfully logged
            Debug.Log("Room clear");
            //Add some code to advance to next scene
        }
        numberOfEnemmies = enemies.Length;
    }
}
