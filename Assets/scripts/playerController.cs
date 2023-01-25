using UnityEngine;
using UnityEngine.InputSystem;

// This code is based on sample unity movement API code.

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class playerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cam;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    public bool isAttacking = false;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main.transform;
        // add actions from playerControls here
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0){
            playerVelocity.y = 0f;
        }

        // store direction input 
        Vector2 input = moveAction.ReadValue<Vector2>();

        // if there is movement input 
        if (input.x != 0 || input.y != 0){
            Vector3 move = new Vector3(input.x, 0, input.y);
            
            // calculate model rotation
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cam.eulerAngles.y; // from front facing position to direction pressed + camera angle.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // apply rotation

            // move according to calculated target angle
            move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(move * Time.deltaTime * playerSpeed);
        }

        // Changes the height position of the player.. 
        if (jumpAction.triggered && groundedPlayer){
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        // add gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
        // attacking
        if (attackAction.triggered){
            if(isAttacking == true){
                isAttacking = false;
            }else{
                Debug.Log("attacking");
                isAttacking = true;
            }
            
        }

    }
}