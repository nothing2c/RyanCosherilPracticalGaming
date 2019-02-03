using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public float maxHealth;
    public float currentHealth;

    // Use this for initialization
    void Awake () {
        maxHealth = 100;
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void adjustHealth(float hit)
    {
        currentHealth += hit;
    }

    public float calculateHealth()// used to change the value attribute of the healthbar slider
    {
        return currentHealth / maxHealth;
    }
}
