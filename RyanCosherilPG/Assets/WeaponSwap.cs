using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string swapWeapon(string currentWeaponType)
    {
        switch(currentWeaponType)
        {
            case "melee":
                //gameObject.GetComponentInChildren<GameObject>().SetActive(false);
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                return "range";
            case "range":
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                return "melee";
        }

        return "";
        //foreach(Transform child in transform)
        //{
            
        //}
    }
}
