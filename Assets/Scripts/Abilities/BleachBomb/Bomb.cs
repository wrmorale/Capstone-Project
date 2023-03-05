using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Projectile
{
    protected override void Update()
    {
        base.Update();
        transform.position += heading * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") 
        {
            Debug.Log("projectile hit");
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.isHit(damage);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("projectile destroyed");
    }
}
