﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsDrop : MonoBehaviour {

    int value;
    GoldDisplayUpdater text;
    // Use this for initialization
    void Start () {
        value = (int)(Random.value * 100);
        text = FindObjectOfType<GoldDisplayUpdater>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == GameObject.Find("player").GetComponent<BoxCollider>())
        {
            text.SendMessage("updateText", value);
            Destroy(gameObject);
        }      
    }
}
