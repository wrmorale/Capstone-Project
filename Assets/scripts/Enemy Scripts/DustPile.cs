using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustPile : MonoBehaviour{
    [Header("stats")]
    [SerializeField]public string enemyName;
    [SerializeField]public float maxHealth;
    [SerializeField]public float health;
    [SerializeField]public float range;
    public float maxScale;
    private MeshRenderer meshRenderer;

    public float healingSpeed = 0.2f;
    private float healingTimer = 2f;
    private float healingCountdown;

    void Start(){
       health = maxHealth / 2;
       maxScale = 2;
       meshRenderer = GetComponent<MeshRenderer>();
       healingCountdown = healingTimer;
    }

    private void Update() {
        // Check if there are any nearby enemies
        healingCountdown -= Time.deltaTime;
        if (IsEnemyNearby() || healingCountdown <= 0) {
            healingCountdown = healingTimer;
            // Slowly increase health if not at max health
            if (health < maxHealth) {
                IncreaseHealth(healingSpeed);
            }
        }
    }

    public void isHit(float damage) {
        print("DustPileTookDamage");
        health -= damage;
        if (health <= 0) {
            // Destroy the cube when it has no health left
            
            Destroy(gameObject);
        } else {
            // Decrease scale and opacity
            transform.localScale *= 0.5f;
            Color color = meshRenderer.material.color;
            meshRenderer.material.color = new Color(color.r, color.g, color.b, color.a * 0.5f);
        }
    }

    public void SetHealth(float newHealth) {
        health = newHealth;
        UpdateVisuals();
    }

    public void IncreaseHealth(float amount) {
        health += amount;
        health = Mathf.Clamp(health, 0.0f, maxHealth);
        UpdateVisuals();
    }

    private void UpdateVisuals() {
        // Update the scale and opacity based on health
        float scale = health / maxHealth * maxScale;
        scale = Mathf.Clamp(scale, 0.5f, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);
        Color color = GetComponent<Renderer>().material.color;
        color.a = scale;
        GetComponent<Renderer>().material.color = color;
    }

    private bool IsEnemyNearby() {
        // Find all Enemy objects in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // Check if any of the enemies are within range
        foreach (Enemy enemy in enemies) {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range) {
                return true;
            }
        }

        // No enemies are nearby
        return false;
    }
}
