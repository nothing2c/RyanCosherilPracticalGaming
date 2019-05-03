using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour {

    public Text characterName;
    public Text dialog;
    public Button option1;
    public Button option2;
    public Button option3;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void option1Execute()
    {
        Debug.Log("Option1");
    }

    public void option2Execute()
    {
        Debug.Log("Option2");
    }

    public void option3Execute()
    {
        Debug.Log("Option3");
    }
}
