using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using HudElements;


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
    public static int currRoom = 1;
    private int roomCount = 4;
    public int currentGold; 
    public List<String> availableAbilities = new List<String>(); //not sure how we will keep track of abilities yet but a list of strings to hold ablities that can be learned
    //
    public int numberOfEnemies = 10;
    public float maxDustPiles = 5;
    private float numberOfDustPiles;
    public GameObject enemyPrefab;
    public GameObject player;
    public GameObject spawnArea;
    public GameObject dustPilePrefab;
    public Player playerStats;

    private bool objectsInstantiated = false;

    //UI stuff
    public UIDocument hud;
    private CleaningBar cleaningbar;

    [Range(0,1)]
    public float cleaningPercent = 0;

    private float dustPilesCleaned;

    //setup singleton
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start(){
        timer = 0;
        roomCleared = false;
        currentGold = 0;
        numberOfDustPiles = maxDustPiles;
        //Dust Pile Spawn
        if(!objectsInstantiated){
            Bounds spawnBounds = spawnArea.GetComponent<MeshCollider>().bounds;
            for (int i = 0; i < numberOfDustPiles; i++)
            {
                Vector3 position = new Vector3(
                    UnityEngine.Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                    2.125f,
                    UnityEngine.Random.Range(spawnBounds.min.z, spawnBounds.max.z)
                );
                Instantiate(dustPilePrefab, position, Quaternion.identity);
            }
            //
            //create enemy copies at a location near the player
            Vector3 playerPos = player.transform.position;
            for(int i = 0; i < numberOfEnemies; i++){
                Vector3 position;
                do {
                    position = new Vector3(
                        UnityEngine.Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                        playerPos.y,
                        UnityEngine.Random.Range(spawnBounds.min.z, spawnBounds.max.z)
                    );
                } while (Vector3.Distance(playerPos, position) < 3);
                GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            }

            // Disable original objects
            enemyPrefab.SetActive(false);
            dustPilePrefab.SetActive(false);

            objectsInstantiated = true;
            
        }

        // UI set up
        var root = hud.rootVisualElement;
        cleaningbar = root.Q<CleaningBar>();
        cleaningbar.value = 0;

    }

    void Update(){
        timer += Time.deltaTime;
        //Debug.Log("Time: " + timer.ToString("F2")); //timer displays in console for now

        //Checks to see if enemies are still in arena
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        DustPile[] dustPiles = FindObjectsOfType<DustPile>();
        //deletes the enemy from the array if it has been destroyed
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null)
            {
                Array.Clear(enemies, i, 1);
            }
        }
        if(!playerStats.alive && playerStats.lives ==1){
            playerStats.lives--;
            //Debug.Log("You're Dead, Loser");
            //here we could insert a scene jump to a losing scene
        }
        if(enemies.Length == 0 && dustPiles.Length == 0 && !roomCleared){
            roomCleared = true; 
            //Room clear condition successfully logged
            Debug.Log("Room clear");
            //Add some code to advance to next scene
            if (currRoom < roomCount) {
                Debug.Log(currRoom);
                currRoom++;
                SceneManager.LoadScene("room_" + currRoom);
            } else {
                // show end credits, player went through all rooms.
            }
        }
        numberOfEnemies = enemies.Length;
        numberOfDustPiles = dustPiles.Length;
        dustPilesCleaned = maxDustPiles - numberOfDustPiles;

        numberOfDustPiles = Mathf.Clamp(numberOfDustPiles, 0, maxDustPiles);
        cleaningPercent = dustPilesCleaned/maxDustPiles;
        cleaningbar.value = cleaningPercent;


    }
    

}
