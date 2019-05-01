using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    float damageMultiplier;
    float damage;
    public RangedWeapon firedFrom;
    bool isCollided;
    float survivalTime;
    Rigidbody rb;
    public float shotForce;
    BoxCollider col;

	// Use this for initialization
	void Start () {
        damageMultiplier = 1.5f;
        damage = firedFrom.damage * damageMultiplier;
        rb = gameObject.GetComponent<Rigidbody>();
        survivalTime = 3;
        isCollided = false;
        col = gameObject.GetComponent<BoxCollider>();

        rb.AddForce(transform.forward * shotForce, ForceMode.Impulse);
        Debug.Log(damage);
    }
	
	// Update is called once per frame
	void Update () {
        if(!isCollided)
        {
            transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime); 
        }

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
        if(!isCollided)
        {
            if (other != firedFrom.transform.root.GetComponent<BoxCollider>())
            {
                if (other.gameObject.GetComponent<Health>())
                {
                    other.gameObject.SendMessage("damage", damage);
                }
                stick(other.transform);
            }
        }
    }

    private void stick(Transform stickTarget)
    {
        transform.parent = (stickTarget.transform);
        Destroy(rb);
        isCollided = true;
    }
}
