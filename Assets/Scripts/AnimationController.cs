using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    Animator animator;
    float volocity_z = 0.0f;
    float volocity_x = 0.0f;
    public float accelaration = 0.1f;
    public float decelaration = 0.1f;

    public float WalkMaxSpeed = 0.5f;
    public float RunMaxSpeed = 1.0f;

    bool ForwardPressed;
    bool RunPressed;
    bool LeftPressed;
    bool RightPressed;

    float currentMaxSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void HandleMovement(float currentMaxSpeed, bool ForwardPressed, bool LeftPressed, bool RightPressed)
    {
        if(ForwardPressed && volocity_z <= currentMaxSpeed)
        {
            volocity_z += Time.deltaTime * accelaration;
        }

        if(!ForwardPressed && volocity_z >= 0.0f)
        {
            volocity_z -= Time.deltaTime * decelaration;
        }
        
        if(ForwardPressed && volocity_z > currentMaxSpeed)
        {
            volocity_z -= Time.deltaTime * decelaration;
        }

        if(LeftPressed && volocity_x >= -1.0f)
        {
            volocity_x -= Time.deltaTime * accelaration;
        }

        if(!LeftPressed && volocity_x <= 0.0f)
        {
            volocity_x += Time.deltaTime * decelaration;
        }

        if(RightPressed && volocity_x <= 1.0f)
        {
            volocity_x += Time.deltaTime * accelaration;
        }

        if(!RightPressed && volocity_x >= 0.0f)
        {
            volocity_x -= Time.deltaTime * decelaration;
        }
    }

    void CapSpeed(bool LeftPressed, bool RightPressed)
    {
        if(volocity_z < 0.0f)
            volocity_z = 0.0f;

        if(volocity_z > 1.0f)
            volocity_z = 1.0f;
        
        if(!LeftPressed && !RightPressed && Mathf.Abs(volocity_x - 0.0f) <= 0.05f)
        {
            volocity_x = 0.0f;
        }

        if(volocity_x > 1.0f)
        {
            volocity_x = 1.0f;
        }

        if(volocity_x < -1.0f)
        {
            volocity_x = -1.0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        ForwardPressed = Input.GetKey(KeyCode.W);
        RunPressed = Input.GetKey(KeyCode.LeftShift);
        LeftPressed = Input.GetKey(KeyCode.A);
        RightPressed = Input.GetKey(KeyCode.D);

        currentMaxSpeed = RunPressed ? RunMaxSpeed : WalkMaxSpeed; 

        HandleMovement(currentMaxSpeed, ForwardPressed, LeftPressed, RightPressed);
        CapSpeed(LeftPressed,RightPressed);

        animator.SetFloat("Volocity Z", volocity_z);
        animator.SetFloat("Volocity X", volocity_x);
    }
}
