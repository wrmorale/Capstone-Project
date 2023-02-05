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
    public void onAttackCancelFrameStart() {
        // set state to start next attack to start new coroutine
        // StopCoroutine(player.coroutine);
    }
    public void onAttackCancelFrameEnd() { }
    public void onAllCancelFrameStart() { }
    public void onAllCancelFrameEnd() { }
    public void onLastFrameStart(){
        Debug.Log("onLastFrameStart");
        light1Clip.animator.SetBool("Attacking", false);
        player.SetState(States.PlayerStates.Idle);
        player.justEnded = true;
        combo = 0;
    }
    public void onLastFrameEnd(){
        Debug.Log("onLastFrameEnd");
    }

    void Awake()
    {
        player = gameObject.GetComponent<playerController>();

        light1Clip.initialize();
        light1FrameChecker.initialize(this, light1Clip);
    }
    

    // This custom update function can be called every frame from the Update() in playerController.cs to reduce overhead.
    // Only call if the player's state is Attacking.

    private void updateMe() // do we need this?
    {
        Debug.Log("Combo " + combo);
        light1FrameChecker.checkFrames();
    }

    public IEnumerator handleAttacks(){
        int frames = 0;                   // amount of frames in anim 

        // first attack
        if(combo == 0){ 
            light1FrameChecker.checkFrames();
            frames = light1Clip.getTotalFrames();
            combo++;
            // start animation
            light1Clip.animator.SetBool("Attacking", true); 
            Debug.Log("attacking set true");
            light1FrameChecker.initCheck();

            // Take this out of if statement ?
            for (int i = 0; i < frames; i++){
                updateMe();
                yield return new WaitForSeconds(0.033f); // return at the right frame interval
            }

        }
    }
}