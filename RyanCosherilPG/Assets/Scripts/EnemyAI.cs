using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float aggroRange;
    public GameObject player;
    public enum States {idle, patroling, attacking, chasing};
    States currentState;
    public enum Transitions {seeSomething, lostSight, inRange, atRange, none};
    Transitions currentTransition;
    public NavMeshAgent navMeshAgent;
    // Use this for initialization
    void Start () {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        currentState = States.idle;
        currentTransition = Transitions.none;
        aggroRange = 10f;
	}
	
	// Update is called once per frame
	void Update () {

        setCurrentTransition();

		switch(currentState)
        {
            case States.idle:
                switch (currentTransition)
                {
                    case Transitions.seeSomething:
                        currentState = States.chasing;
                        break;
                }
                break;

            case States.patroling:
                switch(currentTransition)
                {
                    case Transitions.seeSomething:
                        currentState = States.chasing;
                        break;
                }
                break;
        }
               
	}

    public void setCurrentState(States state)
    {
        this.currentState = state;
    }

    public void setCurrentTransition()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= aggroRange)
        {
            Debug.Log("sup");
            navMeshAgent.SetDestination(player.transform.position);
        }
            
    }
}
