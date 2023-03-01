using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pState = States.PlayerStates;
using aState = States.ActionState;

public class FeatherDusterTriggerable : PlayerAbility, IFrameCheckHandler
{
    public override float baseCooldown
    {
        get { return 2f; }
    }
    [SerializeField] private Duster projectile;
    public Transform bulletSpawn;
    [SerializeField] private float speed = 250f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private FrameParser throwClip;
    [SerializeField] private FrameChecker throwFrameChecker;
    [SerializeField] private float damage = 1f;
    private playerController player;
    private Vector3 playerForward;
    private aState state;
    public void onActiveFrameStart()
    {
    }
    public void onActiveFrameEnd()
    {
    }
    public void onAttackCancelFrameStart()
    {
        state = aState.AttackCancelable;
    }
    public void onAttackCancelFrameEnd()
    {
        if (state == aState.AttackCancelable) { state = aState.Inactionable; }
    }
    public void onAllCancelFrameStart()
    {
        state = aState.AllCancelable;
    }
    public void onAllCancelFrameEnd()
    {
        state = aState.Inactionable;
    }
    public void onLastFrameStart()
    {
    }
    public void onLastFrameEnd()
    {
    }

    void Awake()
    {
        player = gameObject.GetComponent<playerController>();
        player.SetState(pState.Rolling);

        throwClip.initialize();
        throwFrameChecker.initialize(this, throwClip);
        playerForward = player.transform.forward;
        state = aState.Inactionable;
    }

    public void updateMe(float time) 
    {
        
    }
    public override void Activate()
    {
        throwClip.animator.SetBool("Throwing", true);
        throwClip.animator.Play("throw", 0);
        throwFrameChecker.initCheck();
        throwFrameChecker.checkFrames();
        cooldownTimer = baseCooldown;
    }

    public override void UpdateCooldown(float time)
    {
        cooldownTimer -= time;
        cooldownTimer = Mathf.Max(0f, cooldownTimer);
    }
    public void SpawnProjectile(Vector3 heading) 
    {
        Duster duster = Instantiate(projectile, bulletSpawn.position, transform.rotation) as Duster;
        duster.Initialize(speed, lifetime, damage, heading);
    }
}
