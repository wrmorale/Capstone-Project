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

    private playerController player;
    private int combo = 0;
    enum ActionState {Inactionable, AttackCancelable, AllCancelable}
    private ActionState actionState;
    //atack frame data management
    public void onActiveFrameStart() {
        //call hitbox detection
        Debug.Log("onActiveFrameStart");
        weapon.SetActive(true);
    }
    public void onActiveFrameEnd() {
        Debug.Log("onActiveFrameEnd");
        weapon.SetActive(false);
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
        light1Clip.animator.SetBool("Attacking", false);
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
    }
    

    // This custom update function can be called every frame from the Update() in playerController.cs to reduce overhead.
    // Only call if the player's state is Attacking.

    private void updateMe() // do we need this?
    {
        Debug.Log("Combo " + combo);
        if (combo == 1) light1Checker.checkFrames();
        else if (combo == 2) light2Checker.checkFrames();
        else if (combo == 3) light3Checker.checkFrames();
        
        if (player.attackAction.triggered)
        {
            if (actionState == ActionState.Inactionable)
            {
            }
            else if (actionState == ActionState.AttackCancelable)
            {
                StopAllCoroutines();
                actionState = ActionState.Inactionable;
                StartCoroutine(handleAttacks());
            }
            else if (actionState == ActionState.AllCancelable)
            {
                StopAllCoroutines();
                combo = 0;
                actionState = ActionState.Inactionable;
                StartCoroutine(handleAttacks());
            }
        }
        
    }

    public IEnumerator handleAttacks(){
        int frames = 0;                   // amount of frames in anim 
        actionState = ActionState.Inactionable;
        // first attack
        if (combo == 0)
        {
            light1Checker.checkFrames();
            frames = light1Clip.getTotalFrames();
            combo++;
            // start animation
            light1Clip.animator.SetBool("Attacking", true);
            Debug.Log("attacking set true");
            light1Checker.initCheck();
        }
        else if (combo == 1)
        {
            light2Checker.checkFrames();
            frames = light2Clip.getTotalFrames();
            combo++;
            light2Clip.animator.SetBool("Attacking", true);
            light2Checker.initCheck();
        }
        else if (combo == 2)
        {
            light3Checker.checkFrames();
            frames = light3Clip.getTotalFrames();
            combo++;
            light3Clip.animator.SetBool("Attacking", true);
            light3Checker.initCheck();
        }
        for (int i = 0; i < frames; i++)
        {
            updateMe();
            yield return new WaitForSeconds(0.033f); // return at the right frame interval
        }
    }
}