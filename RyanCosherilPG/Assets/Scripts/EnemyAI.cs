using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float aggroRange;
    public float meleeRange;
    public float searchTimer;
    public GameObject player;
    public enum States {idle, searching, attacking, chasing};
    States currentState;
    public enum Transitions {seeSomething, lostSight, inMeleeRange, outOfMeleeRange, deAgro, none};
    List<Transitions> currentPossibleTransitions;
    Transitions currentTransition;
    public NavMeshAgent navMeshAgent;
    Animator animate;
    Vector3 spawnPosition;
    // Use this for initialization
    void Start () {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        currentState = States.idle;
        currentTransition = Transitions.none;
        currentPossibleTransitions = new List<Transitions>();
        aggroRange = 10f;
        meleeRange = 1.5f;
        animate = gameObject.GetComponent<Animator>();
        spawnPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        setCurrentTransition();
        setCurrentState();
        
        switch(currentState)
        {
            case States.idle:
                if(Vector3.Distance(transform.position, spawnPosition) > .1f)
                {
                    navMeshAgent.SetDestination(spawnPosition);
                    animate.SetBool("NeedToMove", true);
                }
                else
                {
                    animate.SetBool("NeedToMove", false);
                }

                animate.SetBool("SeeSomething", false);
                break;
            case States.searching:
                if (!navMeshAgent.pathPending)
                {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            animate.SetBool("NeedToMove", false);
                        }
                    }
                }

                searchTimer -= Time.deltaTime;
                animate.SetBool("SeeSomething", true);
                break;
            case States.attacking:
                navMeshAgent.isStopped = true;
                animate.SetBool("IsAttacking", true);
                break;
            case States.chasing:
                navMeshAgent.isStopped = false;
                animate.SetBool("NeedToMove", true);
                navMeshAgent.SetDestination(player.transform.position);
                break;
        }
	}

    public void setCurrentState(Transitions transition)
    {
        currentTransition = transition;
        setCurrentState();
    }

    public void setCurrentState()
    {
        switch (currentState)
        {
            case States.idle:
                switch (currentTransition)
                {
                    case Transitions.seeSomething:
                        currentState = States.chasing;
                        break;
                }
                break;

            case States.chasing:
                {
                    switch (currentTransition)
                    {
                        case Transitions.inMeleeRange:
                            currentState = States.attacking;
                            break;
                        case Transitions.lostSight:
                            searchTimer = 5;
                            currentState = States.searching;
                            break;
                    }
                }
                break;

            case States.attacking:
                {
                    switch (currentTransition)
                    {
                        case Transitions.outOfMeleeRange:
                            currentState = States.chasing;
                            break;
                    }
                }
                break;

            case States.searching:
                switch (currentTransition)
                {
                    case Transitions.seeSomething:
                        currentState = States.chasing;
                        break;
                    case Transitions.deAgro:
                        currentState = States.idle;
                        break;
                }
                break;
        }
    }

    public void setCurrentTransition()
    {
        switch(currentState)
        {
            case States.idle:
                if (isFacing() && inAggroRange())
                {
                    if (canSee())
                        currentTransition = Transitions.seeSomething;
                    else
                        currentTransition = Transitions.none;
                }
                else
                    currentTransition = Transitions.none;
                break;

            case States.chasing:
                if (inMeleeRange())
                    currentTransition = Transitions.inMeleeRange;
                else if (!canSee())
                    currentTransition = Transitions.lostSight;
                else
                    currentTransition = Transitions.none;
                break;

            case States.attacking:
                if (!inMeleeRange())
                    currentTransition = Transitions.outOfMeleeRange;
                else
                    currentTransition = Transitions.none;
                break;

            case States.searching:
                if (inAggroRange() && canSee())
                {
                    currentTransition = Transitions.seeSomething;
                }
                else if(searchTimer <= 0)
                {
                    currentTransition = Transitions.deAgro;
                }
                else
                    currentTransition = Transitions.none;
                break;

            
        }
    }

    public bool inAggroRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange)
            return true;
        else
            return false;
    }

    public bool isFacing()
    {
        if (Vector3.Dot(player.transform.position - transform.position, transform.forward) > 0)
            return true;
        else
            return false;
    }

    public bool canSee()
    {
        RaycastHit info;
        Debug.DrawRay(transform.position + Vector3.up, (player.transform.position - transform.position), Color.black, 100);

        if (Physics.Raycast(transform.position + Vector3.up, (player.transform.position - transform.position), out info, 100))
        {
            if (info.transform.gameObject == player)
                return true;
        }

        return false;
    }

    public bool inMeleeRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position + Vector3.up) <= meleeRange) 
            return true;   
        else
            return false;
    }
}
