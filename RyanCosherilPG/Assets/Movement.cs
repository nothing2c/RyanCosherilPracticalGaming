using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    Vector3 direction;
    Vector3 playerForward;
    Vector3 cameraStartPosition;
    float speed;
    enum States {idle, moving, jumping}
    States currentState;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        currentState = States.idle;
        playerForward = transform.forward;
        speed = 2;
        cameraStartPosition = Camera.main.transform.position;
        //gameObject.AddComponent<Rigidbody>();
        //rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case States.idle:
                if (shouldMoveForward())
                    moveForward();
                if (shouldTurnLeft())
                    turnLeft();
                if (shouldTurnRight())
                    turnRight();
                if (shouldTurnAround())
                    turnAround();
                if(isIdle())
                    direction = Vector3.zero;
                    currentState = States.idle;
                break;
            case States.moving:
                if (shouldMoveForward())
                    moveForward();
                if (shouldTurnLeft())
                    turnLeft();
                if (shouldTurnRight())
                    turnRight();
                if (shouldTurnAround())
                    turnAround();
                if (isIdle())
                    direction = Vector3.zero;
                    currentState = States.idle;
                break;
        }

        Camera.main.transform.position = transform.position + cameraStartPosition;
    }

    private bool isIdle()
    {
        if (!shouldMoveForward() && !shouldTurnLeft() && !shouldTurnRight() && !shouldTurnAround())          
            return true;
            
        else
            return false;
    }

    private void move()
    {
        transform.position += speed * direction * Time.deltaTime;
        currentState = States.moving;
    }

    private bool shouldMoveForward()
    {
        return Input.GetKey("w");
    }

    private void moveForward()
    {
        direction = transform.forward;
        move();
    }

    private bool shouldTurnLeft()
    {
        return Input.GetKey("a");
    }

    private void turnLeft()
    {
        direction = -transform.right;
        if (transform.rotation.y != -90)
        {
            Debug.Log("Yas");
        }
            transform.Rotate(0,-90,0);
        move();
    }

    private bool shouldTurnRight()
    {
        return Input.GetKey("d");
    }

    private void turnRight()
    {
        direction = transform.right;
        move();
    }

    private bool shouldTurnAround()
    {
        return Input.GetKey("s");
    }

    private void turnAround()
    {
        direction = -transform.forward;
        move();
    }

    private bool shouldJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void jump()
    {

    }
}
