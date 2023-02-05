using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Extensions;
using States;
// This code is based on sample unity movement API code.

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class playerController : MonoBehaviour, IFrameCheckHandler
{
    [SerializeField]
    private float playerSpeed = 2.5f;
    [SerializeField]
    private float walkSpeed = 1.5f;
    [SerializeField]
    private float walkThreshold = 0.5f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    public float turnSmoothTime = 0.1f;
    [SerializeField]
    private FrameParser jumpClip;
    [SerializeField]
    private FrameChecker jumpFrameChecker;

    float turnSmoothVelocity;

    private CharacterController controller;
    private PlayerInput playerInput;
    private BroomAttackManager attackManager;
    private Vector3 playerVelocity;
    private float lastY;
    private bool groundedPlayer;
    private bool inJumpsquat = false;
    public bool justEnded = false;

    public IEnumerator coroutine;

    private Transform cam;

    public InputAction moveAction;
    public InputAction walkAction;
    public InputAction jumpAction;
    public InputAction attackAction;

    public States.PlayerStates state;

    


    //Animation stuff
    Animator animator;

    // jump framedata management
    public void onActiveFrameStart() 
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
    public void onActiveFrameEnd() 
    {
        inJumpsquat = false;
    }
    public void onAttackCancelFrameStart() { }
    public void onAttackCancelFrameEnd() { }
    public void onAllCancelFrameStart() { }
    public void onAllCancelFrameEnd() { }
    public void onLastFrameStart(){}
    public void onLastFrameEnd(){}


    private void Awake()
    {
        controller  = gameObject.GetComponent<CharacterController>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        animator    = gameObject.GetComponent<Animator>();
        attackManager = gameObject.GetComponent<BroomAttackManager>();
        cam = Camera.main.transform;
        // add actions from playerControls here
        moveAction   = playerInput.actions["Run"];
        jumpAction   = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        walkAction   = playerInput.actions["Walk"];
        jumpClip.initialize();
        jumpFrameChecker.initialize(this, jumpClip);
        SetState(States.PlayerStates.Idle);
        // coroutine = attackManager.handleAttacks();
    }

    void Update()
    {
        //Debug.Log(state);
        lastY = controller.transform.position.y;
        jumpFrameChecker.checkFrames();
        groundedPlayer = controller.isGrounded;

        // handle edge case where player lands without ever gaining downward velocity
        if (groundedPlayer && !inJumpsquat) { animator.SetBool("Jumping", false); }
        // stop falling animation on land
        if (groundedPlayer && playerVelocity.y < 0){
            playerVelocity.y = -2.0f;
            animator.SetBool("Falling", false);
        }

        // store direction input 
        Vector2 input = moveAction.ReadValue<Vector2>();

        // if there is movement input 

        if (state == States.PlayerStates.Attacking) {
            attackManager.updateMe();
        }
        if(state != States.PlayerStates.Attacking){
            //StopCoroutine(coroutine);
            //coroutine = attackManager.handleAttacks();
            if (input.x != 0 || input.y != 0){
                bool walking = false;
                Vector3 move = new Vector3(input.x, 0, input.y);
                if (move.magnitude < walkThreshold || walkAction.triggered) { walking = true; }

                // calculate model rotation
                float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y; // from front facing position to direction pressed + camera angle.
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f); // apply rotation

                // move according to calculated target angle
                move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (walking) {
                    controller.Move(move * Time.deltaTime * walkSpeed);
                    animator.SetBool("Walking", true);
                    animator.SetBool("Running", false);
                }
                else {
                    controller.Move(move * Time.deltaTime * playerSpeed);
                    animator.SetBool("Running", true);
                    animator.SetBool("Walking", false);
                }
            }
            else
            {
                animator.SetBool("Running", false);
                animator.SetBool("Walking", false);
            }

            // Changes the height position of the player
            if (jumpAction.triggered && groundedPlayer){
                // playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                animator.SetBool("Jumping", true);
                inJumpsquat = true;
                jumpFrameChecker.initCheck();
            }

            // animate falling player
            if (controller.transform.position.y <= lastY && !groundedPlayer && !inJumpsquat)
            {
                //Debug.Log(transform.position.y);
                //Debug.Log(lastY);
                animator.SetBool("Jumping", false);
                animator.SetBool("Falling", true);
            }

            if (attackAction.triggered)
            {
                // launch animations and attacks
                // if (state != States.PlayerStates.Attacking) StartCoroutine(coroutine);
                //set state to attacking 
                attackManager.handleAttacks();
                SetState(States.PlayerStates.Attacking);
            }
        }

        //TO DO: check if the player is in a valid attack state
        
        
        // add gravity
        ApplyGravity();
        justEnded = false;
    }

    private void ApplyGravity(){
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void SetState(States.PlayerStates newState){
        state = newState;
    }
}