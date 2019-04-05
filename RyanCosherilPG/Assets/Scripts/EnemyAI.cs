using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float aggroRange;
    public float meleeRange;
    public GameObject player;
    public enum States {idle, searching, attacking, chasing};
    States currentState;
    public enum Transitions {seeSomething, lostSight, inMeleeRange, outOfMeleeRange, deAgro, none};
    List<Transitions> currentPossibleTransitions;
    Transitions currentTransition;
    public NavMeshAgent navMeshAgent;
    Animator animate;
    Vector3 lastKnownLocation;
    // Use this for initialization
    void Start () {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        currentState = States.idle;
        currentTransition = Transitions.none;
        currentPossibleTransitions = new List<Transitions>();
        aggroRange = 10f;
        meleeRange = 2f;
        animate = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        currentPossibleTransitions = getPossibleTransitions();
        setCurrentState(currentTransition);
        
        switch(currentState)
        {
            case States.idle:
                animate.SetBool("SeeSomething", false);
                break;
            case States.searching:
                animate.SetBool("SeeSomething", true);
                break;
            case States.attacking:
                Debug.Log("inRange");
                animate.SetBool("InRange", true);
                break;
            case States.chasing:
                animate.SetBool("SeeSomething", true);
                navMeshAgent.SetDestination(player.transform.position);
                break;
        }
	}

    public void setCurrentState(Transitions transition)
    {
        switch (currentState)
        {
            case States.idle:
                switch (transition)
                {
                    case Transitions.seeSomething:
                        currentState = States.chasing;
                        break;
                }
                break;

            case States.searching:
                switch (transition)
                {
                    case Transitions.seeSomething:
                        currentState = States.chasing;
                        break;
                }
                break;
            case States.attacking:
                {

                }
                break;
            case States.chasing:
                {
                    switch (transition)
                    {
                        case Transitions.inMeleeRange:
                            currentState = States.attacking;
                            break;
                        case Transitions.lostSight:
                            currentState = States.searching;
                                break;
                    }
                
                }
                break;
        }
    }

    public List<Transitions> getPossibleTransitions()
    {
        List<Transitions> possibleTransitions = new List<Transitions>();

        if (inAggroRange() && isFacing())
        {
            if (canSee())
                possibleTransitions.Add(Transitions.seeSomething);
            else
                possibleTransitions.Add(Transitions.lostSight);
        }

        if (inMeleeRange())
            possibleTransitions.Add(Transitions.inMeleeRange);
        else
            possibleTransitions.Add(Transitions.outOfMeleeRange);

        //if()

        return possibleTransitions;
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

        if (Physics.Raycast(transform.position, transform.forward, out info, 100))
        {
            if (info.transform.gameObject == player)
                return true;
        }

        return false;
    }

    public bool inMeleeRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
            return true;
        else
            return false;
    }

    public void searchTimer()
    {

    }
}
