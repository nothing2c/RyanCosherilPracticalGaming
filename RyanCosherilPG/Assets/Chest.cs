using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour {

    int gold;
    bool isInteractable;
    // Use this for initialization
    void Start () {
        gold = (int)(Random.value * 100);
        isInteractable = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void interacted()
    {
        if(isInteractable)
        {
            GoldDisplayUpdater text = GameObject.Find("GoldDisplay").GetComponent<GoldDisplayUpdater>();
            text.SendMessage("updateText", gold);
            isInteractable = false;
        }
    }
}
