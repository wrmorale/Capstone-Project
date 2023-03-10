using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using Extensions;

public class GolemAttackManager : MonoBehaviour, IFrameCheckHandler
{
    [SerializeField]
    public GameObject light1Collider;
    [SerializeField]
    public GameObject light2Collider;
    [SerializeField]
    public GameObject spinAttackCollider;
    [SerializeField]
    public GameObject dashCollider;
    [SerializeField]
    private FrameParser light1Clip;
    [SerializeField]
    private FrameChecker light1Checker;
    [SerializeField]
    private FrameParser light2Clip;
    [SerializeField]
    private FrameChecker light2Checker;
    [SerializeField]
    private FrameParser spinAttackClip;
    [SerializeField]
    private FrameChecker spinAttackChecker;
    [SerializeField]
    private FrameParser dashClip;
    [SerializeField]
    private FrameChecker dashChecker;

    private FrameParser activeClip;
    private FrameChecker activeChecker;

    private Enemy enemyInstance;
    private string currentAttack;

    enum ActionState {Inactionable, AttackCancelable}
    private ActionState actionState;

    public void onActiveFrameStart() {
        light1Collider.SetActive(true);
    }
    public void onActiveFrameEnd() {
        light1Collider.SetActive(false);
    }
    public void onAttackCancelFrameStart() {
        actionState = ActionState.AttackCancelable;
        //have it so that it can cast another attack after the last attack
        //if it wants to
    }
    public void onAttackCancelFrameEnd() {
        if (actionState == ActionState.AttackCancelable) actionState = ActionState.Inactionable;
    }
    public void onAllCancelFrameStart(){}
    public void onAllCancelFrameEnd(){}
    public void onLastFrameStart(){}
    public void onLastFrameEnd(){
        activeClip.animator.SetBool("Light1", false);
    }

    void Awake()
    {
        light1Clip.initialize();
        light1Checker.initialize(this, light1Clip);
        light2Clip.initialize();
        light2Checker.initialize(this, light2Clip);
        spinAttackClip.initialize();
        spinAttackChecker.initialize(this, spinAttackClip);
        dashClip.initialize();
        dashChecker.initialize(this, dashClip);

        activeChecker = light1Checker;
        activeClip = light1Clip;
    }

    public void updateMe(float time) // yes we need this
    {
        activeChecker.checkFrames();
        if (actionState == ActionState.Inactionable){}
        if (actionState == ActionState.AttackCancelable)
        {
            actionState = ActionState.Inactionable;
            //handleAttacks();
        }
    }
    public void handleAttacks(Ability ability)
    {
        int frames = 0; // amount of frames in anim 
        actionState = ActionState.Inactionable;

        // first attack
        if (ability.abilityName == "Light1")
        {
            activeChecker = light1Checker;
            activeClip = light1Clip;
        }
        else if (ability.abilityName == "Light2")
        {
            activeChecker = light2Checker;
            activeClip = light2Clip;
        }
        else if (ability.abilityName == "Spin")
        {
            activeChecker = spinAttackChecker;
            activeClip = spinAttackClip;
        }
        else if (ability.abilityName == "Dash")
        {
            activeChecker = dashChecker;
            activeClip = dashClip;
        }

        frames = activeClip.getTotalFrames();
        activeClip.animator.SetBool("Attacking", true);
        activeClip.animator.Play(activeClip.animatorStateName, 0);
        activeChecker.initCheck();
        activeChecker.checkFrames();
    }
}