using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldDisplayUpdater : MonoBehaviour {

    public Text goldText;
    float totalGold;
    // Use this for initialization
    void Start () {
        totalGold = CharacterStats.gold;
        goldText = GetComponentInParent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateText(int gold)
    {
        totalGold += gold;
        goldText.text = totalGold.ToString();
    }
}
