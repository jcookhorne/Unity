using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    public float speed = 5f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();

        playerInput.CharacterControls.Move.started += context =>
        { 
           currentMovementInput = context.ReadValue<Vector2>();
           currentMovement.x = currentMovementInput.x;
           currentMovement.z = currentMovementInput.y;
           isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

        };

    }
    
    // Update is called once per frame
    void Update() { 
    

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
