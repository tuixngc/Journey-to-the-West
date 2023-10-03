using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    [SerializeField] float initialVolocity = 0.0f;
    float currentVerticalVolocity;
    float gravity = -9.8f;
    Vector3 currentPosition;
    bool isJumping = false;
    float jumpTime = 0.0f;

    bool isGround = true;
    Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    bool isGrounded()
    {
        isGround = Physics.Raycast(transform.position, -Vector3.up, 0.55f);
        return isGround;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        
         if(Input.GetKey("space") && !isJumping && isGround)
            isJumping = true;
        
        Debug.Log("isGrounded : " + isGround.ToString());
        Debug.Log("isJumping : " + isJumping.ToString());

        if(isJumping)
        {
            jumpTime += Time.deltaTime;
            currentVerticalVolocity = initialVolocity + gravity * jumpTime;
            //transform.position.Set(currentPosition.x,(currentPosition.y + currentVerticalVolocity * jumpTime),currentPosition.z);
            newPos = new Vector3(currentPosition.x,(currentPosition.y + currentVerticalVolocity * Time.deltaTime),currentPosition.z);
            transform.position = newPos;

            if(isGrounded())
            {
                isJumping = false;
                jumpTime = 0.0f;
            }
        }

    }
}
