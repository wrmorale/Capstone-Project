using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duster : MonoBehaviour
{
    private float speed = 250f;
    private float lifetime = 1f;
    private float damage = 1f;
    private Vector3 heading = Vector3.forward;
    private Enemy enemy;
    // Start is called before the first frame update
    public void Initialize(float speed, float lifetime, float damage, Vector3 heading)
    {
        this.speed = speed;
        this.lifetime = lifetime;
        this.damage = damage;
        this.heading = heading;
        Destroy(gameObject, this.lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += heading * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") 
        {
            enemy.isHit(damage);
            Destroy(gameObject);
        }
    }
}
