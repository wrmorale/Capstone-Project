using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float speed = 0.1f;
    protected float lifetime = 1f;
    protected float damage = 1f;
    protected float stagger = 1f;
    protected Vector3 heading = Vector3.forward;
    public void Initialize(float speed, float lifetime, float damage, float stagger, Vector3 heading)
    {
        this.speed = speed;
        this.lifetime = lifetime;
        this.damage = damage;
        this.stagger = stagger;
        this.heading = heading;
        Destroy(gameObject, this.lifetime);
    }

    protected virtual void Update()
    {
        
    }
}
