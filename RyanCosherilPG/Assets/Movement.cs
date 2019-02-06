using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    /// <summary>
    /// direction character will move
    /// </summary>
    Vector3 direction;
    Vector3 camToPlayer;
    Vector3 orieantation;
    List<Enemy> possibleTargets;
    Enemy target;
    /// <summary>
    /// speed at which character moves
    /// </summary>
    float speed;
    float lockOnRange;
    /// <summary>
    /// speed at which camera turns
    /// </summary>
    float cameraTurnSpeed;
    enum States {idle, moving, jumping, lockedOn, dead}
    /// <summary>
    /// all possible states of character
    /// </summary>
    States currentState;
    Health myhealth;
    public Slider healthBar;
    //Rigidbody rb;

	// Use this for initialization
	void Start () {
        currentState = States.idle;
        speed = 3;
        lockOnRange = 10;
        cameraTurnSpeed = 10;
        camToPlayer = transform.position - Camera.main.transform.position;
        myhealth = gameObject.AddComponent<Health>();
        possibleTargets = new List<Enemy>();
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
                if(shouldToggleLockOn())
                {
                    aquireTargets();
                    if (canLockOn())
                        lockOn(0);
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
                if (shouldToggleLockOn())
                {
                    aquireTargets();
                    if (canLockOn())
                        lockOn(0);
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
                if (shouldToggleLockOn())
                    breakLock();
                if (shouldMeleeAttack())
                    meleeAttack();
                if (shouldRangedAttack())
                    rangedAttack();
                break;
        }

        if(currentState!=States.dead)
        {
            Camera.main.transform.position = transform.position - camToPlayer;
            Camera.main.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal") * cameraTurnSpeed * Time.deltaTime);
            camToPlayer = transform.position - Camera.main.transform.position;
        }
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
        direction.x = Camera.main.transform.forward.x;
        direction.z = Camera.main.transform.forward.z;
        direction.y = transform.position.y; ;

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
        direction.x = -Camera.main.transform.right.x;
        direction.z = -Camera.main.transform.right.z;
        direction.y = transform.position.y; ;

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
        direction.x = Camera.main.transform.right.x;
        direction.z = Camera.main.transform.right.z;
        direction.y = transform.position.y; ;

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
        direction.x = -Camera.main.transform.forward.x;
        direction.z = -Camera.main.transform.forward.z;
        direction.y = transform.position.y; ;

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

    private bool shouldToggleLockOn()
    {
        return Input.GetKeyDown("f");
    }

    private void aquireTargets()
    {
        foreach (Enemy e in Collections.targets)
        {
            if (Vector3.Dot(Camera.main.transform.forward, e.transform.position) > 0)
            {
                if ((e.transform.position - transform.position).magnitude <= lockOnRange)
                {
                    if (!possibleTargets.Contains(e))
                        possibleTargets.Add(e);
                }   
            }
        }
    }

    private void keepTargets()
    {
        Enemy targetLost = null;

        foreach (Enemy e in possibleTargets)
        {
            if ((e.transform.position - transform.position).magnitude > lockOnRange+1)
                targetLost = e;
        }

        possibleTargets.Remove(targetLost);

        aquireTargets();
    }

    private bool canLockOn()
    {
        if (possibleTargets.Count != 0)
            return true;
        else
            return false;
    }

    private void lockOn (int index)
    {
        try
        {
            target = possibleTargets[index];
        }
        catch(ArgumentOutOfRangeException e)
        {
            if (index == -1)
                target = possibleTargets[possibleTargets.Count-1];
            else
                target = possibleTargets[0];
        }
        
        target.setIsTargeted(true);
        currentState = States.lockedOn;
    }

    private void maintainLock()
    {
        orieantation.x = (target.transform.position.x - transform.position.x);
        orieantation.z = (target.transform.position.z - transform.position.z);
        orieantation.y = transform.position.y;

        transform.forward = orieantation;

        keepTargets();

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            target.setIsTargeted(false);
            lockOn(possibleTargets.IndexOf(target)+1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            target.setIsTargeted(false);
            lockOn(possibleTargets.IndexOf(target) - 1);
        }

        if ((target.transform.position - transform.position).magnitude > lockOnRange+1)
            breakLock();

        //Camera.main.transform.LookAt(target.transform.position);
        //Camera.main.transform.forward = ;
        //Camera.main.transform.position = transform.position - camToPlayer;
    }

    private void breakLock()
    {
        currentState = States.idle;
        target.setIsTargeted(false);
        target = null;
        possibleTargets.Clear();
    }

    private void approach()
    {
        direction = transform.forward;

        transform.position += speed * direction * Time.deltaTime;
    }

    private void strafeLeft()
    {
        direction = -transform.right;

        transform.position += speed * direction * Time.deltaTime;
    }

    private void strafeRight()
    {
        direction = transform.right;

        transform.position += speed * direction * Time.deltaTime;
    }

    private void moveBack()
    {
        direction = -transform.forward;

        transform.position += speed * direction * Time.deltaTime;
    }

    private bool shouldMeleeAttack()
    {
        return Input.GetMouseButtonDown(0);
    }

    /// <summary>
    /// attacks with melee weapon
    /// </summary>
    private void meleeAttack()
    {
        if(target)
            target.SendMessage("damage", -10);
    }

    private bool shouldRangedAttack()
    {
        return Input.GetMouseButtonDown(1);
    }

    /// <summary>
    /// attacks with ranged weapon
    /// </summary>
    private void rangedAttack()
    {
        myhealth.adjustHealth(-10);
        healthBar.value = myhealth.calculateHealth();

        if (myhealth.currentHealth <= 0)
            death();
    }

    void death()
    {
        GameObject.Find("HUD").AddComponent<Text>();
        Text youDied = GameObject.Find("HUD").GetComponent<Text>();
        youDied.text = "You Died";
        youDied.color = Color.black;
        youDied.fontSize = 100;
        youDied.font = Resources.Load<Font>("brotherhood");
        youDied.alignment = TextAnchor.MiddleCenter;

        currentState = States.dead;

        Invoke("reloadScene", 3);
    }

    void reloadScene()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
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
