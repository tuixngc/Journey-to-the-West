using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    [SerializeField] float initialVelocity = 0.0f;
    float currentVerticalVelocity;
    [SerializeField] float gravity = -9.8f;
    Vector3 currentPosition;
    bool isJumping = false;
    float jumpTime = 0.0f;

    bool isGround = true;
    Vector3 newPos;

    void Awake ()
    {
	// 0 for no sync, 1 for panel refresh rate, 2 for 1/2 panel rate
	QualitySettings.vSyncCount = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentVerticalVelocity = initialVelocity;
    }
    
    bool isGrounded()
    {
        return transform.position.y <= 0.55;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        
        if(Input.GetKey("space") && !isJumping && isGround)
            isJumping = true;
        
        //Debug.Log("isGrounded : " + isGround.ToString());
        //Debug.Log("isJumping : " + isJumping.ToString());
        if(isJumping)
        {
            //transform.position.Set(currentPosition.x,(currentPosition.y + currentVerticalVelocity * jumpTime),currentPosition.z);
            newPos = new Vector3(currentPosition.x,(currentPosition.y + currentVerticalVelocity * Time.deltaTime),currentPosition.z);
            transform.position = newPos;
            currentVerticalVelocity = currentVerticalVelocity + gravity * Time.deltaTime;   

            if(isGrounded())
            {
                isJumping = false;
                currentVerticalVelocity = initialVelocity;
            }
        }
        Debug.Log(isGrounded().ToString());

    }
}
