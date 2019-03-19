using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    int damage;
    Enemy target;
    float survivalTime;
    Rigidbody rb;
    public float shotSpeed;

	// Use this for initialization
	void Start () {
        damage = 20;
        rb = gameObject.GetComponent<Rigidbody>();
        survivalTime = 3;

        rb.AddForce(transform.forward * shotSpeed, ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
        transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime);
        arrowTimer();
    }

    void arrowTimer()
    {
        survivalTime -= Time.deltaTime;

        if (survivalTime <= 0)
            Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>())
            target = other.gameObject.GetComponent<Enemy>();
        
        if(target)
        {
            target.damage(damage);
            Destroy(gameObject);
        }
    }
}
