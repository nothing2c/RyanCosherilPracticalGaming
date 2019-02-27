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

    public string swapWeapon(string weaponType)
    {
        switch(weaponType)
        {
            case "melee":
                gameObject.GetComponentInChildren<GameObject>().SetActive(false);
                return "range";
                break;
        }

        return "";
        //foreach(Transform child in transform)
        //{
            
        //}
    }
}
