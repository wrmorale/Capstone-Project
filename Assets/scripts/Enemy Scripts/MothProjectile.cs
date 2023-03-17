using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothProjectile : Projectile
{
    protected override void Update()
    {
        base.Update();
        transform.position += heading * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            Player player = other.GetComponent<Player>();
            player.isHit(damage);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("projectile destroyed");
    }
}
