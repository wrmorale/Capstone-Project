using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool roomCleared;

    //setup singleton
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        roomCleared = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks to see if enemies are still in arena
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if(enemies.Length ==0){
            print("NO ENEMIES");
            roomCleared = true; 
        }
        else{
            print("YES ENEMIES");
        }

    }
}
