using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustPile : MonoBehaviour{
    [Header("stats")]
    [SerializeField]public string enemyName;
    [SerializeField]public double maxHealth;
    [SerializeField]public double health;
    private MeshRenderer meshRenderer;

    void Start(){
       health = maxHealth;
       meshRenderer = GetComponent<MeshRenderer>();
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
}
