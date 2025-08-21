using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isStrafingLeftHash;
    int isStrafingRightHash;
    int isStrafingBackwardHash;


    float rotationFactorPerFrame = 5.0f;
    public float walkMultiplier = 2.0f;
    public float runMultiplier = 6.0f;
    public float jumpForce = 5.0f;
    float gravity = -9.0f;
    float groundedGravity = -0.5f;

    bool isMovementPressed;
    //bool isRunPressed;
    //bool isJumpPressed;

    bool isJumpingup;
    bool isMovingUp;
    bool isMovingDown;
    bool isMovingLeft;
    bool isMovingRight;
    bool isRunning;

    Vector3 currentMovement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isStrafingLeftHash = Animator.StringToHash("isStrafingLeft");
        isStrafingBackwardHash = Animator.StringToHash("isStrafingBackward");
        isStrafingRightHash = Animator.StringToHash("isStrafingRight");
    }

    // Update is called once per frame
    void Update()
    {
        
        handleRotation();
        handleAnimation();
        // Gravity logic
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
        }
        else
        {
            currentMovement.y += gravity * Time.deltaTime;
        }

        // Apply movement and gravity
        characterController.Move(currentMovement * Time.deltaTime * (isRunning ? runMultiplier : walkMultiplier));
       
 

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        // good for controllers because : This event is triggered continuously while the movement input is active.
        //  As long as you hold down a movement key, the performed event fires every frame, providing updated
        //  input values.
        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;

    }

    void handleRotation()
    {
        //Vector3 positionToLookAt;
        //// the change in position our character should point to
        //positionToLookAt.x = currentMovement.x;
        //positionToLookAt.y = 0.0f;
        //positionToLookAt.z = currentMovement.z;

        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        //I want this to rotate with the mouse not the keys
        //if (isMovementPressed)
        //{
        //    // creates a new rotation based on where the plater is currentl pressing
        //    Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        //    transform.rotation =  Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

        //}
    }
    void handleAnimation()
    {
        animator.SetBool(isStrafingLeftHash, isMovingLeft);
        animator.SetBool(isStrafingRightHash, isMovingRight);
        animator.SetBool(isStrafingBackwardHash, isMovingDown);
        animator.SetBool(isWalkingHash, isMovingUp);

        animator.SetBool(isJumpingHash, isJumpingup);
        animator.SetBool(isRunningHash, isRunning);

        //}
    }
    void onRun(InputAction.CallbackContext context)
    {
        // Fix: Use a boolean to check if the run action is triggered instead of trying to read a Button type.
        isRunning = context.ReadValueAsButton();
        characterController.Move(currentMovement * Time.deltaTime * (isRunning ? runMultiplier : walkMultiplier));
        Debug.Log("Is Running: " + isRunning);
    }
    void onMovementInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        currentMovement.x = input.x;
        currentMovement.z = input.y;
        isMovingUp = input.y > 0;
        isMovingDown = input.y < 0;
        isMovingLeft = input.x < 0;
        isMovingRight = input.x > 0;

    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpingup = context.ReadValueAsButton();
        if(isJumpingup)
        {
            currentMovement.y = jumpForce;
       
        }
        Debug.Log("Current Jump isJumpingup: " + isJumpingup);
    }


    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
