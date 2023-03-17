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

    private GameObject broom;
    private GameObject pan;
    private Animator broomAnimator;
    private Animator[] trailAnimators = new Animator[3];

    private playerController player;
    private Vector2 input = Vector2.zero;

    private int combo = 0;
    enum ActionState {Inactionable, AttackCancelable, AllCancelable}
    private ActionState actionState;

    /* Attack frame data management */
    public void onActiveFrameStart() {
        // call hitbox detection
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
        // let the player move between attacks
        if (combo != 0){
            Vector2 input = player.moveAction.ReadValue<Vector2>();
            if (input.x != 0 || input.y != 0){
                Vector3 move = new Vector3(input.x, 0, input.y);
                move = player.RotatePlayer(input);
                player.controller.Move(move * Time.deltaTime * 0.01f);
            }
        }
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
    }
    public void onLastFrameEnd(){
        activeClip.animator.SetBool("Attacking", false);
        player.SetState(States.PlayerStates.Idle);
        broom.SetActive(false);
        pan.SetActive(false);
        combo = 0;
    }

    void Awake()
    {
        player = gameObject.GetComponent<playerController>();
        broom = transform.Find("maid68/metarig/hip/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/broom1").gameObject;
        broomAnimator = broom.GetComponent<Animator>();
        pan = transform.Find("maid68/metarig/hip/spine/chest/shoulder.L/upper_arm.L/forearm.L/hand.L/pan").gameObject;
        trailAnimators[0] = transform.Find("maid68/trail1").GetComponent<Animator>();
        trailAnimators[1] = transform.Find("maid68/trail2").GetComponent<Animator>();
        trailAnimators[2] = transform.Find("maid68/trail3").GetComponent<Animator>();

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

    public void updateMe(float time) // yes we need this
    {
        activeChecker.checkFrames();
        
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
                broom.SetActive(false);
                pan.SetActive(false);
            }
            if (player.channeledAbility >= 0)
            {
                actionState = ActionState.Inactionable;
                combo = 0;
                activeClip.animator.SetBool("Attacking", false);
                player.ActivateAbility();
                player.ResetRoot();
                broom.SetActive(false);
                pan.SetActive(false);
            }
        }
        if (player.state == States.PlayerStates.Attacking) { player.MoveRoot(); }
    }

    public void handleAttacks()
    {
        broom.SetActive(true);
        pan.SetActive(true);
        
        int frames = 0; // amount of frames in anim 
        actionState = ActionState.Inactionable;

        // first attack
        if (combo == 0)
        {
            activeChecker = light1Checker;
            activeClip = light1Clip;
            trailAnimators[combo].Play("trail1", 0, 0.0f);
        }
        else if (combo == 1)
        {
            activeChecker = light2Checker;
            activeClip = light2Clip;
            trailAnimators[combo].Play("trail2", 0, 0.0f);
        }
        else if (combo == 2)
        {
            broomAnimator.Play("light_3", 0);
            activeChecker = light3Checker;
            activeClip = light3Clip;
            trailAnimators[combo].Play("trail3", 0, 0.0f);
        }

        activeClip.animator.SetInteger("Combo", combo);
        combo++;
        if (combo > 2) combo = 0;

        frames = activeClip.getTotalFrames();
        activeClip.animator.SetBool("Attacking", true);
        activeClip.play();
        activeChecker.initCheck();
        activeChecker.checkFrames();
    }
}