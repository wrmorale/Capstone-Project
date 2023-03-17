using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using Extensions;

public class GolemAttackManager : MonoBehaviour, IFrameCheckHandler
{
    [SerializeField]
    private Golem enemyInstance;
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

    private string currentAttack;

    enum ActionState {Inactionable, AttackCancelable}
    private ActionState actionState;

    public void onActiveFrameStart() {
        print("StartFRAME");
        enemyInstance.light1Complete = false;
        //have if statements to see which ability to play here
        if(currentAttack == "Light1"){
            light1Collider.SetActive(true);
        }
        else if(currentAttack == "Light2"){
            light2Collider.SetActive(true);
        }
        else if(currentAttack == "SpinAttack"){
            spinAttackCollider.SetActive(true);
        }
        else if(currentAttack == "Dash"){
            dashCollider.SetActive(true);
        }
    }
    public void onActiveFrameEnd() {
        print("EndFRAME");
        enemyInstance.state = Golem.GolemState.Idle;
        enemyInstance.isDashing = false;
        if(currentAttack == "Light1"){
            enemyInstance.light1Complete = true;
            light1Collider.SetActive(false);
            activeClip.animator.SetBool("Light1", false);
        }
        else if(currentAttack == "Light2"){
            light2Collider.SetActive(false);
            activeClip.animator.SetBool("Light2", false);
        }
        else if(currentAttack == "SpinAttack"){
            spinAttackCollider.SetActive(false);
            activeClip.animator.SetBool("SpinAttack", false);
        }
        else if(currentAttack == "Dash"){
            dashCollider.SetActive(false);
            activeClip.animator.SetBool("Dash", false);
        }
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
        enemyInstance.state = Golem.GolemState.Idle;
        enemyInstance.isDashing = false;
        activeClip.animator.SetBool("Light1", false);
        activeClip.animator.SetBool("Light2", false);
        activeClip.animator.SetBool("SpinAttack", false);
        activeClip.animator.SetBool("Dash", false);
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
        }
    }
    public void handleAttacks(Ability ability)
    {
        int frames = 0; // amount of frames in anim 
        actionState = ActionState.Inactionable;
        enemyInstance.state = Golem.GolemState.Attacking;

        currentAttack = ability.abilityName;

        if (currentAttack == "Light1")
        {
            activeChecker = light1Checker;
            activeClip = light1Clip;
        }
        else if (currentAttack == "Light2")
        {
            activeChecker = light2Checker;
            activeClip = light2Clip;
        }
        else if (currentAttack == "SpinAttack")
        {
            activeChecker = spinAttackChecker;
            activeClip = spinAttackClip;
        }
        else if (currentAttack == "Dash")
        {
            activeChecker = dashChecker;
            activeClip = dashClip;
        }
        frames = activeClip.getTotalFrames();
        activeClip.animator.SetBool(ability.abilityName, true);
        activeClip.animator.Play(activeClip.animatorStateName, 0);
        activeChecker.initCheck();
        activeChecker.checkFrames();
    }
}