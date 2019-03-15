using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour {

    GameObject meleeWeapon;
    GameObject rangedWeapon;

	// Use this for initialization
	void Start () {
        meleeWeapon = transform.GetChild(0).gameObject;
        rangedWeapon = transform.GetChild(1).gameObject;
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
                meleeWeapon.SetActive(false);
                rangedWeapon.SetActive(true);
                return "range";
            case "range":
                rangedWeapon.SetActive(false);
                meleeWeapon.SetActive(true);
                return "melee";
        }

        return "";
    }

    public void equipWeapon(string newWeaponPath, string weaponType)
    {
        if(weaponType.Equals("melee"))
        {
            Destroy(meleeWeapon);
            meleeWeapon = Instantiate((GameObject)Resources.Load(newWeaponPath), transform);
        }
    }
}
