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
    float RunMultiplier = 5.0f;
    float WalkMultiplier = 1.5f;
    float Friction = -6.0f;
    
    bool isWalking;
    bool isRunning;
    bool isJumping;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    bool isWalkingPressed;
    bool isRunningPressed;
    bool isJumpingPressed;

    Vector2 currentMovementInput;
    Vector3 currentWalkVelocity;
    Vector3 currentRunVelocity;

    //jump viariables
    float timeToApex = 0.5f;
    float maxHeightOfJumping = 1.0f;
    float gravity;
    float initialVelocity;
    //To update the vertical velocity
    float currentJumpVelocity;
    float newJumpVelocity;
    float nextJumpVelocity;

    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        playerInput.CharacterControls.Movement.performed += OnMovementInput;
        playerInput.CharacterControls.Movement.started += OnMovementInput;
        playerInput.CharacterControls.Movement.canceled += OnMovementInput;
        playerInput.CharacterControls.Run.started += OnRunInput;
        playerInput.CharacterControls.Run.canceled += OnRunInput;
        playerInput.CharacterControls.Jump.started += OnJumpInput;
        playerInput.CharacterControls.Jump.canceled += OnJumpInput;

        setupJumpVariables();
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        isJumpingPressed = context.ReadValueAsButton();
    }
    
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isWalkingPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
        
        if(!isJumping && characterController.isGrounded)
        {
            currentWalkVelocity.x = currentMovementInput.x * WalkMultiplier;
            currentWalkVelocity.z = currentMovementInput.y * WalkMultiplier;
            currentRunVelocity.x = currentMovementInput.x * RunMultiplier;
            currentRunVelocity.z = currentMovementInput.y * RunMultiplier;
        }
        
    }

    void OnRunInput(InputAction.CallbackContext context)
    {
        bool isRunningInput = context.ReadValueAsButton();
        
        if(!isJumping && characterController.isGrounded)
        {
            isRunningPressed = isRunningInput;
        }
        else
        {
            isRunningPressed = false;
        }
    }

    void handleAnimation()
    {
        isWalking = animator.GetBool(isWalkingHash);
        isRunning = animator.GetBool(isRunningHash);
        isJumping = animator.GetBool(isJumpingHash);

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
        if(isRunningPressed)
        {
            characterController.Move(currentRunVelocity * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentWalkVelocity * Time.deltaTime);
        }
        
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentWalkVelocity.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentWalkVelocity.z;
        
        Quaternion currentRotation = transform.rotation;
        
        if(isWalkingPressed)
        {
            Quaternion TargetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,TargetRotation,RotationPerFrame * Time.deltaTime);
        }

    }
     
    void setupJumpVariables()
    {
        gravity = (-2 * maxHeightOfJumping) / Mathf.Pow(timeToApex, 2.0f);
        initialVelocity = (2 * maxHeightOfJumping) / timeToApex;
    }

    void handleJump()
    {
        if(!isJumping && isJumpingPressed && characterController.isGrounded)
        {
            isJumping = true;
            currentWalkVelocity.y = initialVelocity * 0.5f;
            currentRunVelocity.y = initialVelocity * 0.5f;
        }
        if(isJumping && !isJumpingPressed && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void handleGravity()
    {
        if(characterController.isGrounded)
        {
            float groundedGravity = -0.05f;
            currentWalkVelocity.y = groundedGravity;
            currentRunVelocity.y = groundedGravity;
        }
        else
        {
            currentJumpVelocity = currentWalkVelocity.y;
            newJumpVelocity = currentJumpVelocity + gravity * Time.deltaTime;
            nextJumpVelocity = (currentJumpVelocity + newJumpVelocity) * 0.5f;
            currentWalkVelocity.y = nextJumpVelocity;
            currentRunVelocity.y = nextJumpVelocity;
        }
    }

    void handleFriction()
    {
        if(!isWalkingPressed && characterController.isGrounded)
        {
            currentWalkVelocity.x += currentWalkVelocity.x * Friction * Time.deltaTime;
            currentWalkVelocity.z += currentWalkVelocity.z * Friction * Time.deltaTime;
            currentRunVelocity.x += currentRunVelocity.x * Friction * Time.deltaTime;
            currentRunVelocity.z += currentRunVelocity.x * Friction * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleJump();
        handleAnimation();
        handleMovement();
        handleRotation();
        handleGravity();
        handleFriction();
        //handleJump();
        
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
