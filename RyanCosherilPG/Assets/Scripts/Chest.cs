using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour, Interactable {

    int gold;
    bool isInteractable;
    Animation open;
    // Use this for initialization
    void Start () {
        open = gameObject.GetComponent<Animation>();
        gold = (int)(Random.value * 100);
        isInteractable = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void interact()
    {
        if (isInteractable)
        {
            //GoldDisplayUpdater text = GameObject.Find("GoldDisplay").GetComponent<GoldDisplayUpdater>();
            open.Play("chestOpen");
            //text.SendMessage("updateText", gold);
            isInteractable = false;
        }
    }
}
