using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour, Interactable {

    int gold;
    public bool canInteract;
    Animation open;
    // Use this for initialization
    void Start () {
        open = gameObject.GetComponent<Animation>();
        gold = (int)(Random.value * 100);
        canInteract = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void interact(GameObject interactor)
    {
        GoldDisplayUpdater text = GameObject.Find("GoldDisplay").GetComponent<GoldDisplayUpdater>();
        open.Play("chestOpen");
        text.SendMessage("updateText", gold);
        canInteract = false;
    }

    public bool isInteractable()
    {
        return canInteract;
    }
}
