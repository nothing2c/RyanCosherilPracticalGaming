using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public GameObject weapon;
    string currentWeapon;
    MeleeWeapon mWeapon;
    RangedWeapon rWeapon;
    Animator animate;

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
    float runSpeed;
    float walkSpeed;
    float jumpHeight;
    float lockOnRange;
    /// <summary>
    /// speed at which camera turns
    /// </summary>
    float cameraTurnSpeed;
    enum States {lockedOn, freeRoam, inAir, attacking, shooting, dead}
    /// <summary>
    /// all possible states of character
    /// </summary>
    States currentState;
    Health myhealth;
    public Slider healthBar;
    public Slider powerBar;
    Rigidbody rb;
    Collider col;

    float shotForce;

	// Use this for initialization
	void Start () {
        currentState = States.freeRoam;

        currentWeapon = "melee";

        healthBar = FindObjectOfType<GameManager>().GetComponentInChildren<Slider>();

        rWeapon = gameObject.GetComponentInChildren<RangedWeapon>();
        mWeapon = gameObject.GetComponentInChildren<MeleeWeapon>();
        runSpeed = 6;
        walkSpeed = runSpeed / 2;
        lockOnRange = 10;
        jumpHeight = 5;
        cameraTurnSpeed = 1;

        camToPlayer = transform.position - Camera.main.transform.position;

        myhealth = gameObject.AddComponent<Health>();
        possibleTargets = new List<Enemy>();
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<BoxCollider>();
        animate = GetComponent<Animator>();

        shotForce = 0;

        rWeapon.gameObject.SetActive(false);

        powerBar.gameObject.SetActive(false);
        powerBar.maxValue = rWeapon.maxPower;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("i"))
        {
            if(currentWeapon.Equals("melee"))
            {
                weapon.GetComponent<WeaponSwap>().equipWeapon("Low-Poly Weapons/Prefabs/Sword", "melee");
            }
        }
            
        switch (currentState)
        {
            case States.freeRoam:
                if (isIdle())
                    animate.SetBool("IsMoving", false);
                else
                    animate.SetBool("IsMoving", true);

                if (shouldMoveForward())
                    moveForward();
                if (shouldTurnLeft())
                    turnLeft();
                if (shouldTurnRight())
                    turnRight();
                if (shouldTurnAround())
                    turnAround();

                if (shouldMeleeAttack())
                    meleeAttack();
                if (shouldRangedAttack())
                    rangedAttack();

                if (shouldJump())
                    startJump();

                if(shouldInteract())
                {
                    if (canInteract())
                        interact(canInteract());
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

                if (isIdle())
                    animate.SetBool("IsMoving", false);
                else
                    animate.SetBool("IsMoving", true);

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

            case States.inAir:
                if (shouldMoveForward())
                    moveForward();
                if (shouldTurnLeft())
                    turnLeft();
                if (shouldTurnRight())
                    turnRight();
                if (shouldTurnAround())
                    turnAround();

                if (rb.velocity.y < 0)
                {
                    RaycastHit info;
                    if (Physics.Raycast(transform.position, -transform.up, out info, 1))
                    {
                        currentState = States.freeRoam;
                        animate.SetBool("IsAirborne", false);
                    }
                }
                break;

            case States.attacking:

                if(shouldMeleeAttack())
                {
                    animate.SetBool("AttackQued", true);
                }

                if (animate.GetBool("IsAttacking") == false && animate.GetBool("AttackQued") == false)
                {
                    if (target)
                        currentState = States.lockedOn;
                    else
                        currentState = States.freeRoam;
                }
                break;

            case States.shooting:
                if(Input.GetMouseButton(1))
                {
                    powerBar.maxValue = rWeapon.maxPower;
                    if (shotForce < rWeapon.maxPower)
                    {
                        
                        shotForce += (rWeapon.maxPower/rWeapon.shotChargeSpeed) * Time.deltaTime;
                        
                        Debug.Log(powerBar.value);
                    }
                    powerBar.value = shotForce;
                }

                else
                {
                    if (shotForce < rWeapon.minPower)
                        shotForce = rWeapon.minPower;

                    rWeapon.fire(shotForce);

                    shotForce = 0;

                    powerBar.gameObject.SetActive(false);

                    if (target)
                        currentState = States.lockedOn;
                    else
                        currentState = States.freeRoam;
                }
                break;
            case States.dead:
                death();
                break;
        }

        if(Input.GetKeyDown("p"))
        {
            damage(22);
        }
           
        Camera.main.transform.position = transform.position - camToPlayer;
        Camera.main.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Horizontal") * cameraTurnSpeed);
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
        direction.x = Camera.main.transform.forward.x;
        direction.z = Camera.main.transform.forward.z;
        direction.y = transform.forward.y; ;

        if (transform.forward != direction)
            transform.forward += direction;

        transform.position += runSpeed * direction * Time.deltaTime;
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
        direction.y = -transform.right.y; ;

        if (transform.forward != direction)
            transform.forward += direction;

        transform.position += runSpeed * direction * Time.deltaTime;
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
        direction.y = transform.right.y; ;

        if (transform.forward != direction)
            transform.forward += direction;

        transform.position += runSpeed * direction * Time.deltaTime;
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
        direction.y = -transform.forward.y; ;

        if (transform.forward != direction)
            transform.forward += direction;

        transform.position += runSpeed * direction * Time.deltaTime;
    }

    /// <summary>
    /// checks if jump key is pressed
    /// </summary>
    private bool shouldJump()
    {
        return (Input.GetKeyUp(KeyCode.Space));
    }

    private void startJump()
    {
        animate.SetBool("IsAirborne", true);
    }

    /// <summary>
    /// makes the character jump
    /// </summary>
    /// 
    private void jump()
    {
        currentState = States.inAir;
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    private bool shouldToggleLockOn()
    {
        return Input.GetKeyDown("f");
    }

    private void aquireTargets()
    {
        foreach (Enemy e in GameManager.targets)
        {
            if (Vector3.Dot(e.transform.position - Camera.main.transform.position, Camera.main.transform.forward) > 0)
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
        catch(ArgumentOutOfRangeException)
        {
            if (index == -1)
                target = possibleTargets[possibleTargets.Count-1];
            else
                target = possibleTargets[0];
        }
        
        target.setIsTargeted(true);
        currentState = States.lockedOn;
        animate.SetBool("IsLockedOn", true);
    }

    private void maintainLock()
    {
        orieantation.x = (target.transform.position.x - transform.position.x);
        orieantation.z = (target.transform.position.z - transform.position.z);
        orieantation.y = transform.forward.y;

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
    }

    private void breakLock()
    {
        if(target)
        {
            target.setIsTargeted(false);
        }
        animate.SetBool("IsLockedOn", false);
        target = null;
        possibleTargets.Clear();
        currentState = States.freeRoam;
    }

    private void approach()
    {
        direction = transform.forward; 

        transform.position += walkSpeed * direction * Time.deltaTime;
    }

    private void strafeLeft()
    {
        direction = -transform.right;

        transform.position += walkSpeed * direction * Time.deltaTime;        
    }

    private void strafeRight()
    {
        direction = transform.right;

        transform.position += walkSpeed * direction * Time.deltaTime;
    }

    private void moveBack()
    {
        direction = -transform.forward;

        transform.position += walkSpeed * direction * Time.deltaTime;   
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
        if(!currentWeapon.Equals("melee"))
        {
            currentWeapon = weapon.GetComponent<WeaponSwap>().swapWeapon("range");
        }
            
        else
        {
            animate.applyRootMotion = true;
            animate.SetBool("IsAttacking", true);
            currentState = States.attacking;
        }
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
        if (!currentWeapon.Equals("range"))
        {
            currentWeapon = weapon.GetComponent<WeaponSwap>().swapWeapon("melee");
        }

        else if(Input.GetMouseButton(1))
        {
            currentState = States.shooting;
            powerBar.gameObject.SetActive(true);
        }
    }

    void death()
    {
        animate.applyRootMotion = true;
        animate.SetBool("IsDead", true);
        col.enabled = false;
        Destroy(rb);

        FindObjectOfType<GameManager>().setCurrentGameState(GameManager.GameStates.playerDead);

        enabled = false;
    }

    public void damage(float damage)
    {
        myhealth.adjustHealth(-damage);
        healthBar.value = myhealth.calculateHealth();

        if (myhealth.currentHealth <= 0)
        {
            currentState = States.dead;
        }
    }

    private bool shouldInteract()
    {
        return Input.GetKeyDown("e");
    }

    /// <summary>
    /// checks if any interactable objects can be interacted with
    /// </summary>
    private GameObject canInteract()
    {
        int i = 0;
        Collider[] cols = Physics.OverlapSphere(transform.position, 2f);

        foreach(Collider c in cols)
        {
            if (c.gameObject.GetComponent<Interactable>() != null)
            {
                Debug.Log(i++);
                if (c.gameObject.GetComponent<Interactable>().isInteractable())
                {
                    return c.transform.gameObject;
                }
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    /// <param name="interactedObject">object interacted with</param>
    private void interact(GameObject interactedObject)
    {
        interactedObject.SendMessage("interact", gameObject);
    }
}
