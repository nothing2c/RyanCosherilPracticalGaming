using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingIndicator : MonoBehaviour {

    private GameObject targetingIndicator;
    Vector3 spawnPosition;

	// Use this for initialization
	void Start () {

        targetingIndicator = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TargetIndicator/Indicator"),transform);
        targetingIndicator.transform.parent = transform;
        spawnPosition = new Vector3(0, GetComponentInParent<BoxCollider>().size.y, 0);
        Debug.Log(spawnPosition);
        targetingIndicator.transform.localPosition = spawnPosition * 1.2f;
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
