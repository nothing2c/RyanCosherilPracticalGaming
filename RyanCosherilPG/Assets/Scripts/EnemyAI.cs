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
    public enum Transitions {seeSomething, lostSight, inRange, none};
    Transitions currentTransition;
    public NavMeshAgent navMeshAgent;
    Animator animate;
    Vector3 lastKnownLocation;
    // Use this for initialization
    void Start () {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        currentState = States.idle;
        currentTransition = Transitions.none;
        aggroRange = 10f;
        meleeRange = 5f;
        animate = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        currentTransition = setCurrentTransition();
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
                        case Transitions.inRange:
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

    public Transitions setCurrentTransition()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
            {
                Debug.Log("in range yas");
                return Transitions.inRange;
            }
            else
                return Transitions.seeSomething;
        }

        
            

        return Transitions.lostSight;
    }
}
