using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    float rotationFactorPerFrame = 15.0f;
    bool isRunPressed;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        // good for controllers for responsivness
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Move.started += onRun;
        playerInput.CharacterControls.Move.canceled += onRun;
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        characterController.Move(currentMovement * Time.deltaTime);
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


    void handleAnimation()
    {
        bool isWalking = animator.GetBool("Walk");
        bool isIdle = animator.GetBool("Idle");

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool("isWalking", true);

        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool("isWalking", false);
        }

    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

    }
    
    void onRun (InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
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
