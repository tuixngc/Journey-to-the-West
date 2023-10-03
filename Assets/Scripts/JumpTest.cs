using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    [SerializeField] float initialVolocity = 0.0f;
    float currentVerticalVolocity;
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
        currentVerticalVolocity = initialVolocity;
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
            //transform.position.Set(currentPosition.x,(currentPosition.y + currentVerticalVolocity * jumpTime),currentPosition.z);
            newPos = new Vector3(currentPosition.x,(currentPosition.y + currentVerticalVolocity * Time.deltaTime),currentPosition.z);
            transform.position = newPos;
            currentVerticalVolocity = currentVerticalVolocity + gravity * Time.deltaTime;   

            if(isGrounded())
            {
                isJumping = false;
                currentVerticalVolocity = initialVolocity;
            }
        }
        Debug.Log(isGrounded().ToString());

    }
}
