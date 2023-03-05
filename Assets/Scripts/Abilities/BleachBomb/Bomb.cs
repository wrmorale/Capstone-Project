using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    private Rigidbody body;
    [SerializeField] private Explosion explosion;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && !other.isTrigger) 
        {
            Destroy(gameObject);
        }
    }
    public void launch() 
    {
        Vector3 force = (Vector3.up + transform.forward);
        force.Normalize();
        force *= speed;
        body.AddForce(force);
    }

    private void OnDestroy()
    {
        Explosion clone = Instantiate(explosion, transform);
        clone.Initialize(damage, stagger);
        clone.transform.SetParent(null);
    }
}
