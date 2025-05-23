using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    Vector2 currentMovementInput;

    Vector3 currentMovement;
    Vector3 currentRunMovement;

    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;

    float rotationFactorPerFrame = 15.0f;
    float runMultiplier = 3.0f;
    float gravity = -9.0f;
    float groundedGravity = -0.5f;



    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        // good for controllers for responsivness
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
    }

    // Update is called once per frame
    void Update()
    {
        handleGravity();
        handleRotation();
        handleAnimation();
        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
       
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            // creates a new rotation based on where the plater is currentl pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation =  Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

        }
    }


    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;

        } else
        {
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }
    void handleAnimation() 
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isRunning = animator.GetBool(isRunningHash);


        if(isJumpPressed && !isJumping)
        {
            animator.SetBool(isJumpingHash, true);
        }
        else if(!isJumpPressed)
        {
            animator.SetBool(isJumpingHash, false);
        }

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);

        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;

        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }
    
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
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
