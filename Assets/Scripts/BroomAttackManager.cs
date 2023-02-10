using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using Extensions;

public class BroomAttackManager : MonoBehaviour, IFrameCheckHandler
{
    [SerializeField]
    public GameObject attack1_collider;
    [SerializeField]
    public GameObject attack2_collider;
    [SerializeField]
    public GameObject attack3_collider;
    [SerializeField]
    private FrameParser light1Clip;
    [SerializeField]
    private FrameChecker light1Checker;
    [SerializeField]
    private FrameParser light2Clip;
    [SerializeField]
    private FrameChecker light2Checker;
    [SerializeField]
    private FrameParser light3Clip;
    [SerializeField]
    private FrameChecker light3Checker;

    private FrameParser activeClip;
    private FrameChecker activeChecker;

    private playerController player;
    private int combo = 0;
    enum ActionState {Inactionable, AttackCancelable, AllCancelable}
    private ActionState actionState;
    //atack frame data management
    public void onActiveFrameStart() {
        //call hitbox detection
        Debug.Log("onActiveFrameStart");
        if (combo == 1){
            attack1_collider.SetActive(true);
        }
        else if (combo == 2){
            attack2_collider.SetActive(true);
        }
        else if (combo == 0){
            attack3_collider.SetActive(true);
        }
    }
    public void onActiveFrameEnd() {
        Debug.Log("onActiveFrameEnd");
        if (combo == 1){
            attack1_collider.SetActive(false);
        }
        else if (combo == 2){
            attack2_collider.SetActive(false);
        }
        else if (combo == 0){
            attack3_collider.SetActive(false);
        }
    }
    public void onAttackCancelFrameStart() {
        actionState = ActionState.AttackCancelable;
    }
    public void onAttackCancelFrameEnd() {
        if (actionState == ActionState.AttackCancelable) actionState = ActionState.Inactionable;
    }
    public void onAllCancelFrameStart() {
        actionState = ActionState.AllCancelable;
    }
    public void onAllCancelFrameEnd() {
        if (actionState == ActionState.AllCancelable) actionState = ActionState.Inactionable;
    }
    public void onLastFrameStart(){
        Debug.Log("onLastFrameStart");
        
    }
    public void onLastFrameEnd(){
        Debug.Log("onLastFrameEnd");
        activeClip.animator.SetBool("Attacking", false);
        player.SetState(States.PlayerStates.Idle);
        player.justEnded = true;
        combo = 0;
    }

    void Awake()
    {
        player = gameObject.GetComponent<playerController>();

        light1Clip.initialize();
        light1Checker.initialize(this, light1Clip);
        light2Clip.initialize();
        light2Checker.initialize(this, light2Clip);
        light3Clip.initialize();
        light3Checker.initialize(this, light3Clip);
        activeChecker = light1Checker;
        activeClip = light1Clip;
    }
    

    // This custom update function can be called every frame from the Update() in playerController.cs to reduce overhead.
    // Only call if the player's state is Attacking.

    public void updateMe() // yes we need this
    {
        activeChecker.checkFrames();
        player.MoveRoot();
        if (actionState == ActionState.Inactionable)
        { 
        }
        if (actionState == ActionState.AttackCancelable)
        {
            if (player.attackAction.triggered)
            {
                actionState = ActionState.Inactionable;
                handleAttacks();
            }
        }
        if (actionState == ActionState.AllCancelable)
        {
            /*
            if (player.attackAction.triggered)
            {
                actionState = ActionState.Inactionable;
                combo = 0;
                // handleAttacks();
            }
            */
            if (player.jumpAction.triggered)
            {
                actionState = ActionState.Inactionable;
                combo = 0;
                activeClip.animator.SetBool("Attacking", false);
                player.Jump();
            }
        }
    }

    public void handleAttacks(){
        int frames = 0;                   // amount of frames in anim 
        actionState = ActionState.Inactionable;
        // first attack
        if (combo == 0)
        {
            activeChecker = light1Checker;
            activeClip = light1Clip;
        }
        else if (combo == 1)
        {
            activeChecker = light2Checker;
            activeClip = light2Clip;
        }
        else if (combo == 2)
        {
            activeChecker = light3Checker;
            activeClip = light3Clip;
        }
        activeClip.animator.SetInteger("Combo", combo);
        combo++;
        if (combo > 2) combo = 0;
        frames = activeClip.getTotalFrames();
        activeClip.animator.SetBool("Attacking", true);
        activeClip.animator.Play(activeClip.animatorStateName, 0);
        activeChecker.initCheck();
        activeChecker.checkFrames();
    }
}