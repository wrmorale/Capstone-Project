using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
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
    public bool isNextToExit;
    public bool gamePaused;
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
    public GameObject doorPortal;
    public GameObject spawnArea;
    public GameObject dustPilePrefab;
    public GameObject pauseUI;
    public Player playerStats;

    private bool objectsInstantiated = false;

    [SerializeField] private PlayerInput playerInput;
    private InputAction pauseAction;

    //UI stuff
    public UIDocument hud;
    private CleaningBar cleaningbar;

    [Range(0,1)]
    public float cleaningPercent = 0;

    private float dustPilesCleaned;
    private float dirtyingRate = 0.3f; // rate at which the room gets dirty
    private float dustMaxHealth;
    private float pooledHealth;
    private float totalHealth;

    private float fogDensity;

    //setup singleton
    private void Awake() {

        // if(instance != null){
        //     Destroy(this.gameObject);
        //     return;
        // }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        // Adds the pause button to the script
        pauseAction = playerInput.actions["Pause"];

        timer = 0;
        roomCleared = false;
        isNextToExit = false;
        gamePaused = false;
        currentGold = 0;
        numberOfDustPiles = maxDustPiles;
        float spawnAreaY = spawnArea.transform.position.y;
        //Dust Pile Spawn
        if(!objectsInstantiated){
            Bounds spawnBounds = spawnArea.GetComponent<MeshCollider>().bounds;
            for (int i = 0; i < numberOfDustPiles; i++)
            {
                Vector3 position = new Vector3(
                    UnityEngine.Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                    spawnAreaY,
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
                        spawnAreaY,
                        UnityEngine.Random.Range(spawnBounds.min.z, spawnBounds.max.z)
                    );
                } while (Vector3.Distance(playerPos, position) < 3);
                GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            }
            
            dustMaxHealth = dustPilePrefab.GetComponent<DustPile>().maxHealth;

            // Disable original objects
            enemyPrefab.SetActive(false);
            dustPilePrefab.SetActive(false);

            objectsInstantiated = true;
            
        }

        // UI set up
        var root = hud.rootVisualElement;
        Debug.Log("root: " + root);
        cleaningbar = root.Q<CleaningBar>();
        Debug.Log("cleaningbar: "+cleaningbar);
        totalHealth = maxDustPiles * dustPilePrefab.GetComponent<DustPile>().maxHealth;
        cleaningbar.value = totalHealth * 0.5f / totalHealth;

        // fog
        RenderSettings.fog = true;
        fogDensity = cleaningbar.value;

        // Start executing function after 2.0f, and re-execute every 2.0f
        InvokeRepeating("DecreaseCleanliness", 2.0f, 2.0f);

    }

    void Update(){
        timer += Time.deltaTime;
        //Debug.Log("Time: " + timer.ToString("F2")); //timer displays in console for now

        HandleRoomTransition();

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
        if (!playerStats.alive && playerStats.lives == 1 ||
        cleaningbar.value == 0){
            playerStats.lives--;
            //Debug.Log("You're Dead, Loser");
            //here we could insert a scene jump to a losing scene
        }
        if (enemies.Length == 0 && dustPiles.Length == 0 && !roomCleared){
            roomCleared = true;
            doorPortal.SetActive(true);
            //Room clear condition successfully logged
            Debug.Log("Room clear");
            //Add some code to advance to next scene
            // if (currRoom < roomCount) {
            //     Debug.Log(currRoom);
            //     currRoom++;
            //     SceneManager.LoadScene("room_" + currRoom);
            // } else {
            //     // show end credits, player went through all rooms.
            // }
        }
        numberOfEnemies = enemies.Length;
        numberOfDustPiles = dustPiles.Length;

        if (numberOfDustPiles == 0) {
            cleaningbar.value = 1;
        }
        var newPooledHealth = PoolDustHealth(dustPiles);
        if (newPooledHealth < pooledHealth) {
            cleaningbar.value += (pooledHealth - newPooledHealth) / totalHealth;
        }
        pooledHealth = newPooledHealth; // get the current health pool of dustpiles.

        // Adjust Fog based on dustpile health values.
        RenderSettings.fogDensity = pooledHealth / (maxDustPiles * dustMaxHealth) * 0.2f;

        // Checks if player paused the game, if so stops time 
        if ((pauseUI) && pauseAction.triggered){
            if (gamePaused) {
                gamePaused = false;
                pauseUI.SetActive(false);
                Time.timeScale = 1;
            } else {
                gamePaused = true;
                // pauseUI.GetComponent<Popup_Setup>().enabled = true;
                pauseUI.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if ((pauseUI) && (gamePaused == true && (pauseUI.activeSelf == false))){
            gamePaused = false;
        }
    }

    void HandleRoomTransition() {
        if (roomCleared && isNextToExit) {
            isNextToExit = false;
            doorPortal.SetActive(false);
            if (currRoom < roomCount) {
                //Debug.Log(currRoom);
                currRoom++;
                SceneManager.LoadScene("room_" + currRoom);
            } else {
                // show end credits, player went through all rooms.
            }
        }
    }

    private float PoolDustHealth(DustPile[] dustPiles) {
        float pooledHealth = 0f;
        foreach (DustPile dustPile in dustPiles) {
            pooledHealth += dustPile.GetComponent<DustPile>().health;
        }
        return pooledHealth;
    }

    private void DecreaseCleanliness() {
        cleaningbar.value -= dirtyingRate * numberOfDustPiles / totalHealth;
    }

}
