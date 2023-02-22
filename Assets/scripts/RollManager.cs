using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollManager : MonoBehaviour, IFrameCheckHandler
{
    [Header("Roll Settings")] 
    [SerializeField]
    private float rollSpeedMult; // multiplies player running speed.
    [SerializeField]
    private float rollDist;

    [Header("Animator")] 
    [SerializeField]
    private FrameParser rollClip;
    [SerializeField]
    private FrameChecker rollFrameChecker;


    private playerController player;
    private Player playerManager;

    public void onActiveFrameStart() {
        Debug.Log("Rolling now");
        playerManager.isInvulnerable = true;
        rollSpeedMult = player.playerSpeed * 2f; // set roll speed
    }
    public void onActiveFrameEnd() {
    }
    public void onAttackCancelFrameStart() { }
    public void onAttackCancelFrameEnd() { }
    public void onAllCancelFrameStart() { }
    public void onAllCancelFrameEnd() { }
    public void onLastFrameStart(){
        rollClip.animator.SetBool("Rolling", false);
        player.SetState(States.PlayerStates.Idle);
        playerManager.isInvulnerable = false;
    }
    public void onLastFrameEnd(){
        rollClip.animator.SetBool("Rolling", false);
        player.SetState(States.PlayerStates.Idle);
        Debug.Log("rolling ended");
    }

    // Start is called before the first frame update
    void Awake()
    {
        player = gameObject.GetComponent<playerController>();
        playerManager = gameObject.GetComponent<Player>();

        rollSpeedMult = 0; // scale initial speed off player speed.

        rollClip.initialize();
        rollFrameChecker.initialize(this, rollClip);
    }

    // Update is called once per frame
    public void updateMe()
    {
        rollFrameChecker.checkFrames();
        player.MoveRoot();

        player.controller.Move(transform.forward * Time.deltaTime * rollSpeedMult);
        rollSpeedMult -= 1f * Time.deltaTime;
        // use roll speed set onActiveFrame instead. 
    }

    public void Roll()
    {
        rollClip.animator.SetBool("Rolling", true);
        rollClip.animator.Play("roll", 0);
        rollFrameChecker.initCheck();
        rollFrameChecker.checkFrames();
    }
}
