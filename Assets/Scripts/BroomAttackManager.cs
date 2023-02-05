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
    private FrameChecker light1FrameChecker;
    /*[SerializeField]
    private FrameParser light2Clip;
    [SerializeField]
    private FrameChecker light2Checker;
    [SerializeField]
    private FrameParser light3Clip;
    [SerializeField]
    private FrameChecker light3checker;
    */

    private playerController player;
    private int combo = 0;
    enum Actions {Inactionable, AttackCancelable, AllCancelable}

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
    public void onAttackCancelFrameStart() { }
    public void onAttackCancelFrameEnd() { }
    public void onAllCancelFrameStart() { }
    public void onAllCancelFrameEnd() { }
    public void onLastFrameStart(){}
    public void onLastFrameEnd(){
        Debug.Log("onLastFrameEnd");
        light1Clip.animator.SetBool("Attacking", false);
        player.SetState(States.PlayerStates.Idle);
        combo = 0;
    }
    void Awake()
    {
        player = gameObject.GetComponent<playerController>();

        light1Clip.initialize();
        light1FrameChecker.initialize(this, light1Clip);
    }
    

    // This custom update function can be called every frame from the Update() in playerController.cs to reduce overhead.
    // Only call if the player's state is Attacking.
    public void updateMe()
    {   
        Debug.Log("Combo " + combo);
        light1FrameChecker.checkFrames();
        //Let the player rotate (Can make the rotate function from playerController into a proper function)
        if(combo == 0){
            combo++;
            light1Clip.animator.SetBool("Attacking", true);
            light1FrameChecker.initCheck();
        }


    }
}