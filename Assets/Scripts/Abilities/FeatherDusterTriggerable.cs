using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherDusterTriggerable : MonoBehaviour, IFrameCheckHandler
{
    [HideInInspector] public Rigidbody projectile;
    public Transform bulletSpawn;
    [HideInInspector] public float speed = 250f;
    [HideInInspector] public float lifetime = 1f;
    [HideInInspector] public FrameParser throwClip;
    [HideInInspector] public FrameChecker throwFrameChecker;
    [HideInInspector] public float damage = 1f;
    private playerController player;

    public void onActiveFrameStart()
    {
    }
    public void onActiveFrameEnd()
    {
    }
    public void onAttackCancelFrameStart()
    {
    }
    public void onAttackCancelFrameEnd()
    {
    }
    public void onAllCancelFrameStart()
    {
    }
    public void onAllCancelFrameEnd()
    {
    }
    public void onLastFrameStart()
    {
    }
    public void onLastFrameEnd()
    {
    }

    void Awake()
    {
        
    }
    public void Launch()
    {
        Rigidbody clonedBullet = Instantiate(projectile, bulletSpawn.position, transform.rotation) as Rigidbody;

        clonedBullet.AddForce(bulletSpawn.transform.forward * speed);
    }
}
