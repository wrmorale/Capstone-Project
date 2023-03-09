using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using HudElements;

public class Player : MonoBehaviour
{
    

    //this will keep track of stats for player
    [Header("stats")]
    [SerializeField]public float maxHealth;
    [SerializeField]public float movementSpeed;
    [SerializeField]public float basicDamage;
    [SerializeField]public float attackSpeed;
    [SerializeField]public float cooldownReduction;
    public bool alive;
    public int lives;
    [SerializeField]public float health;
    public List<PlayerAbility> abilities; 
    // public Transform platform;
    // public float fallLimit = -10; 
    
    //UI stuff
    public UIDocument hud;
    private HealthBar healthbar;
    
    [Range(0,1)]
    public float healthPercent = 1;

    public bool isInvulnerable;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        alive = true;
        isInvulnerable = false;


        var root = hud.rootVisualElement;
        healthbar = root.Q<HealthBar>();
        healthbar.value = health / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        instead of this, we can create a prefab "kill plane" that's scaled up by a huge factor,
        and has a script attached to detect player (or any object) colliding with it and kill them
        if (transform.position.y < platform.position.y + fallLimit){
            health = 0;
            alive = false;
        }
        */
    }

    public void isHit(float damage){
        //print("Player took " + damage + " damage");
        if(!isInvulnerable){
            health -= damage;
            health = Mathf.Clamp(health, 0 , maxHealth);
            healthPercent = health / maxHealth;
            healthbar.value = healthPercent;  
            if(health <= 0){
                alive = false;
            }
        }
        
    }

}
