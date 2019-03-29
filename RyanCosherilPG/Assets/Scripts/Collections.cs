using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collections : MonoBehaviour {

    public static List<Enemy> targets;
	// Use this for initialization
	void Awake () {
        targets = new List<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
