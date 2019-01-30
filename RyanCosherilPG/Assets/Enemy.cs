using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private bool isTargeted;
    private TargetingIndicator indicator;

	// Use this for initialization
	void Start () {
        isTargeted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isTargeted)
        {
            if(!indicator)
                indicator = gameObject.AddComponent<TargetingIndicator>();
        }
        else
        {
            if (indicator)        
                Destroy(gameObject.GetComponent<TargetingIndicator>());
        }
	}

    public void setIsTargeted(bool targeted)
    {
        this.isTargeted = targeted;
    }
}
