using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerAndAnimationController : MonoBehaviour
{

    Animator animator;
    PlayerInput playerInput;
    CharacterController characterController;
    float RotationPerFrame = 15.0f;
    float RunMultiplier = 3.0f;
    
    bool isWalking;
    bool isRunning;

    int isWalkingHash;
    int isRunningHash;

    bool isWalkingPressed;
    bool isRunningPressed;
    bool isJumpPressed;

    Vector2 currentMovementInput;
    Vector3 currentMovement;

    void Awake()
    {
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        playerInput.CharacterControls.Movement.performed += OnMovementInput;
        playerInput.CharacterControls.Movement.started += OnMovementInput;
        playerInput.CharacterControls.Movement.canceled += OnMovementInput;
        playerInput.CharacterControls.Run.started += OnRunInput;
        playerInput.CharacterControls.Run.canceled += OnRunInput;
        playerInput.CharacterControls.Jump.started += OnJumpInput;
        playerInput.CharacterControls.Jump.canceled += OnJumpInput;
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        
    }
    
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();

        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isWalkingPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void OnRunInput(InputAction.CallbackContext context)
    {
        isRunningPressed = context.ReadValueAsButton();
    }

    void handleAnimation()
    {
        isWalking = animator.GetBool(isWalkingHash);
        isRunning = animator.GetBool(isRunningHash);

        if(isWalkingPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash,true);
        }

        if(!isWalkingPressed && isWalking)
        {
            animator.SetBool(isWalkingHash,false);
        }

        if(isRunningPressed && isWalking && !isRunning)
        {
            animator.SetBool(isRunningHash,true);
        }
        
        if(!isWalking || (!isRunningPressed && isRunning))
        {
            animator.SetBool(isRunningHash,false);
        }
    }

    void handleMovement()
    {
        characterController.Move(currentMovement * Time.deltaTime);
        if(isRunning)
        {
            characterController.Move(currentMovement * Time.deltaTime * RunMultiplier);
        }

        handleGravity();
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        
        Quaternion currentRotation = transform.rotation;
        
        if(isWalkingPressed)
        {
            Quaternion TargetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,TargetRotation,RotationPerFrame * Time.deltaTime);
        }

    }

    void handleGravity()
    {
        if(characterController.isGrounded)
        {
            float groundedGravity = -0.05f;
            currentMovement.y = groundedGravity;
        }
        else
        {
            float ungroundedGravity = -9.8f;
            currentMovement.y += ungroundedGravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleAnimation();
        handleMovement();
        handleRotation();
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
