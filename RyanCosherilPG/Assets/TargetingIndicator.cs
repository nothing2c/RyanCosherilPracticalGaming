using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingIndicator : MonoBehaviour {

    private GameObject targetingIndicator;

	// Use this for initialization
	void Start () {

        targetingIndicator = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TargetIndicator/Indicator"),transform);
    }
	
	// Update is called once per frame
	void Update () {

       targetingIndicator.transform.rotation = Quaternion.LookRotation((-Camera.main.transform.position + targetingIndicator.transform.position).normalized);
    }

    private void OnDestroy()
    {
        Destroy(GameObject.Find("Indicator(Clone)"));
    }
}
