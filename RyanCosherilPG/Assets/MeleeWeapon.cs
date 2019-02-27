using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour {

    CapsuleCollider collider;
	// Use this for initialization
	void Start () {
        collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponentInParent<Animator>().GetBool("IsAttacking"))
            collider.enabled = true;
        else
            collider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Debug.Log("Attacking");
            collision.gameObject.SendMessage("damage", -40f);
        }       
    }
}
