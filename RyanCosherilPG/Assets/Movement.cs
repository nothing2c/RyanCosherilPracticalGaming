﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// direction character will move
    /// </summary>
    Vector3 direction;
    Vector3 camToPlayer;
    GameObject target;
    /// <summary>
    /// speed at which character moves
    /// </summary>
    float speed;
    /// <summary>
    /// speed at which camera turns
    /// </summary>
    float cameraTurnSpeed;
    enum States {idle, moving, jumping, lockedOn}
    /// <summary>
    /// all possible states of character
    /// </summary>
    States currentState;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("enemy");
        currentState = States.idle;
        speed = 2;
        cameraTurnSpeed = 10;
        camToPlayer = transform.position - Camera.main.transform.position;
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
                if (isIdle())
                {
                    direction = Vector3.zero;
                    currentState = States.idle;
                }
                if(shouldLockOn())
                {
                    if (canLockOn())
                        lockOn();
                }
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
                {
                    direction = Vector3.zero;
                    currentState = States.idle;
                }
                if (shouldLockOn())
                {
                    if (canLockOn())
                        lockOn();
                }
                break;
            case States.lockedOn:
                maintainLock();

                if (shouldMoveForward())
                    approach();
                if (shouldTurnLeft())
                    strafeLeft();
                if (shouldTurnRight())
                    strafeRight();
                if (shouldTurnAround())
                    moveBack();
                if (shouldLockOn())
                    breakLock();
                break;
        }
        Debug.Log(currentState);

        Camera.main.transform.position = transform.position - camToPlayer;

        if(currentState != States.lockedOn)
            Camera.main.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal") * cameraTurnSpeed * Time.deltaTime);

        camToPlayer = transform.position - Camera.main.transform.position;
    }

    /// <summary>
    /// checks if no movement input is detected and stops moving
    /// </summary>
    private bool isIdle()
    {
        if (!shouldMoveForward() && !shouldTurnLeft() && !shouldTurnRight() && !shouldTurnAround())
            return true;

        else
            return false;
    }

    /// <summary>
    /// check if move forward key is pressed
    /// </summary>
    private bool shouldMoveForward()
    {
        return Input.GetKey("w");
    }

    /// <summary>
    /// moves character forward
    /// </summary>
    private void moveForward()
    {
        direction.x = Camera.main.transform.forward.x;     //<-----------Theres an idea here
        direction.z = Camera.main.transform.forward.z;
        direction.y = 0;

        if (transform.forward != direction)
            transform.forward = direction;

        transform.position += speed * direction * Time.deltaTime;
        currentState = States.moving;
    }

    /// <summary>
    /// checks if turn left key is pressed
    /// </summary>
    private bool shouldTurnLeft()
    {
        return Input.GetKey("a");
    }

    /// <summary>
    /// turns the character to face left of the camera
    /// </summary>
    private void turnLeft()
    {
        direction.x = -Camera.main.transform.right.x;     //<-----------Theres an idea here
        direction.z = -Camera.main.transform.right.z;
        direction.y = 0;

        if (transform.forward != direction)
            transform.forward = direction;
        
        transform.position += speed * direction * Time.deltaTime;
        currentState = States.moving;
    }

    /// <summary>
    /// checks if turn right key is pressed
    /// </summary>
    private bool shouldTurnRight()
    {
        return Input.GetKey("d");
    }

    /// <summary>
    /// turns the character to face right of the camera
    /// </summary>
    private void turnRight()
    {
        direction.x = Camera.main.transform.right.x;     //<-----------Theres an idea here
        direction.z = Camera.main.transform.right.z;
        direction.y = 0;

        if (transform.forward != direction)
            transform.forward = direction;

        transform.position += speed * direction * Time.deltaTime;
        currentState = States.moving;
    }

    /// <summary>
    /// checks if turn around key is pressed
    /// </summary>
    private bool shouldTurnAround()
    {
        return Input.GetKey("s");
    }

    /// <summary>
    /// turns the character around to face camera
    /// </summary>
    private void turnAround()
    {
        direction.x = -Camera.main.transform.forward.x;     //<-----------Theres an idea here
        direction.z = -Camera.main.transform.forward.z;
        direction.y = 0;

        if (transform.forward != direction)
            transform.forward = direction;

        transform.position += speed * direction * Time.deltaTime;
        currentState = States.moving;
    }

    /// <summary>
    /// checks if jump key is pressed
    /// </summary>
    private bool shouldJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    /// <summary>
    /// makes the character jump
    /// </summary>
    private void jump()
    {

    }

    private bool shouldLockOn()
    {
        return Input.GetKeyDown("f");
    }

    private bool canLockOn()
    {
        if(Vector3.Dot(Camera.main.transform.forward, target.transform.position) >0)
        {
            if ((target.transform.position - transform.position).magnitude <= 8)
                return true;
        }

        return false;
    }

    private void lockOn ()
    {
        //Camera.main.transform.forward = target.transform.position - transform.position;
        Debug.Log("locked on");
        target.GetComponent<Enemy>().setIsTargeted(true);
        currentState = States.lockedOn;
    }

    private void maintainLock()
    {
        transform.forward = target.transform.position - transform.position;
        Camera.main.transform.forward = transform.forward;
    }

    private void breakLock()
    {
        currentState = States.idle;
    }

    private void approach()
    {
        transform.position += speed * Camera.main.transform.forward * Time.deltaTime;
    }

    private void strafeLeft()
    {
        transform.position += speed * -Camera.main.transform.right * Time.deltaTime;
    }

    private void strafeRight()
    {
        transform.position += speed * Camera.main.transform.right * Time.deltaTime;
    }

    private void moveBack()
    {
        transform.position += speed * -Camera.main.transform.forward * Time.deltaTime;
    }

    /// <summary>
    /// attacks with melee weapon
    /// </summary>
    private void meleeAttack()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// attacks with ranged weapon
    /// </summary>
    private void rangedAttack()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// checks if any interactable objects can be interacted with
    /// </summary>
    private bool canInteract()
    {
        throw new System.NotImplementedException();
    }

    /// <param name="interactedObject">object interacted with</param>
    private void interact(GameObject interactedObject)
    {
        throw new System.NotImplementedException();
    }
}
