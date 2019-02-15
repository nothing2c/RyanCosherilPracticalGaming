using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleAgentScript : MonoBehaviour {

    NavMeshAgent navMeshAgent;
    public Transform target;
	// Use this for initialization
	void Start () {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.Find("Sphere").transform;
	}
	
	// Update is called once per frame
	void Update () {
        navMeshAgent.SetDestination(target.position);
	}
}
