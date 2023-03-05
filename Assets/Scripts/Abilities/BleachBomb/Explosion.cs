using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float damage;
    private float stagger;
    [SerializeField] private float lifetime = 0.8f;

    public void Initialize(float damage, float stagger)
    {
        this.damage = damage;
        this.stagger = stagger;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") 
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.isHit(damage);
        }
        if (other.tag == "DustPile")
        {
            DustPile dustPile = other.GetComponent<DustPile>();
            dustPile.isHit(damage);
        }
        if (other.tag == "Furniture")
        {
            Furniture furniture = other.GetComponent<Furniture>();
            furniture.isHit(damage);
        }
    }

}
