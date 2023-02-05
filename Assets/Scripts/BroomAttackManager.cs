using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using Extensions;

public class BroomAttackManager : MonoBehaviour, IFrameCheckHandler
{
    [SerializeField]
    public GameObject weapon;
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
        //weapon.SetActive(true);
    }
    public void onActiveFrameEnd() {
        Debug.Log("onActiveFrameEnd");
        //weapon.SetActive(false);
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

    public void updateMe() // do we need this?
    {
        activeChecker.checkFrames();
        if (player.attackAction.triggered)
        {
            if (actionState == ActionState.Inactionable)
            {
            }
            else if (actionState == ActionState.AttackCancelable)
            {
                actionState = ActionState.Inactionable;
                handleAttacks();
            }
            else if (actionState == ActionState.AllCancelable)
            {
                combo = 0;
                actionState = ActionState.Inactionable;
                handleAttacks();
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
        activeChecker.initCheck();
        activeChecker.checkFrames();
        /*
        for (int i = 0; i < frames; i++)
        {
            updateMe();
            yield return new WaitForSeconds(0.033f); // return at the right frame interval
        }
        */
    }
}