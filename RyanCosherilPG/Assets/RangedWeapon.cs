using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour {

    public float maxPower;
    public float minPower;
    public float shotChargeSpeed;
    public float damage;

    public GameObject shotSpawn;
	// Use this for initialization
	void Start () {
        maxPower = 100;
        minPower = 10;
        shotChargeSpeed = 5;
        damage = 20;

        shotSpawn.transform.forward = Vector3.forward;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void fire(float shotForce)
    {
        Arrow shot = Instantiate(Resources.Load<GameObject>("Low-Poly Weapons/Prefabs/Arrow_Regular"), shotSpawn.transform.position, shotSpawn.transform.rotation).GetComponent<Arrow>();
        shot.firedFrom = this;
        shot.shotForce = shotForce;
    }
}
