using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

//this will keep track of stats for player across scenes. Also helpful for testing since stats can be changed with sliders
[System.Serializable]
public class playerStats
{
    public float maxHealth;
    public float movementSpeed;
    public float basicDamage;
    public float attackSpeed;
    public float cooldownReduction;
}

public class GameManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        roomCleared = false;
        currentGold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Debug.Log("Time: " + timer.ToString("F2")); //timer displays in console for now

        //Checks to see if enemies are still in arena
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if(enemies.Length == 0){
            roomCleared = true; 
            //Add some code to advance to next scene
        }
    }
}
