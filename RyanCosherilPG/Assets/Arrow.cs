using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    int damage;
    float survivalTime;
    Rigidbody rb;
    float shotSpeed;

	// Use this for initialization
	void Start () {
        damage = 20;
        shotSpeed = 20;
        rb = gameObject.GetComponent<Rigidbody>();
        survivalTime = 3;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * shotSpeed * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime);
        arrowTimer();

        //https://www.youtube.com/watch?v=ERh6NCivzJE
    }

    void arrowTimer()
    {
        survivalTime -= Time.deltaTime;

        if (survivalTime <= 0)
            Destroy(gameObject);

    }
}
