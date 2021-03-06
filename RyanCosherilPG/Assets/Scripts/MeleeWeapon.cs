﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour {

    CapsuleCollider col;
    float damage;
	// Use this for initialization
	void Start () {
        col = gameObject.GetComponent<CapsuleCollider>();
        col.enabled = false;
        damage = 40;
	}
	
	// Update is called once per frame
	void Update () {
        //if (gameObject.GetComponentInParent<Animator>().GetBool("IsAttacking"))
        //    collider.enabled = true;
        //else
        //    collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Health>())
        {
            other.gameObject.SendMessage("damage", damage);
            EndAttack.hasHit = true;
        }       
    }
}
