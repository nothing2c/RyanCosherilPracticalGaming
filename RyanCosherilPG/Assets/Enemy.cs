using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private bool isTargeted;

	// Use this for initialization
	void Start () {
        isTargeted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isTargeted)
        {
            transform.Rotate(Vector3.up, 90 * Time.deltaTime);
        }
	}

    public void setIsTargeted(bool targeted)
    {
        this.isTargeted = targeted;
    }
}
