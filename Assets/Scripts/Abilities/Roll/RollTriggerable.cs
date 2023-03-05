using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pState = States.PlayerStates;
using aState = States.ActionState;

public class RollTriggerable : PlayerAbility, IFrameCheckHandler
{
    [SerializeField] private FrameParser clip;
    [SerializeField] private FrameChecker frameChecker;
    
    private playerController player;
    private Player playerManager;
    private aState state;
    public void onActiveFrameStart()
    {
        playerManager.isInvulnerable = true;
    }
    public void onActiveFrameEnd()
    {
        playerManager.isInvulnerable = false;
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
        clip.animator.SetBool("Rolling", false);
        playerManager.isInvulnerable = false;
        player.SetState(States.PlayerStates.Idle);
    }


    public override void updateMe(float time) 
    {
        frameChecker.checkFrames();
        player.controller.Move(player.transform.forward * time * player.playerSpeed * 2f);
    }
    public override void Activate()
    {
        clip.animator.SetBool("Rolling", true);
        clip.play();
        player.SetState(pState.Rolling);
        state = aState.Inactionable;
        frameChecker.initCheck();
        frameChecker.checkFrames();
        cooldownTimer = baseCooldown;
    }

    /*
    public override void UpdateCooldown(float time)
    {
        cooldownTimer -= time;
        cooldownTimer = Mathf.Max(0f, cooldownTimer);
    }
    */

    public override void Initialize(playerController player, Animator animator) 
    {
        this.player = player;
        playerManager = player.GetComponent<Player>();
        clip.animator = animator;
        clip.initialize();
        frameChecker.initialize(this, clip);
    }
}
