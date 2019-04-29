using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour {

    public string characterName;
    List<string> dialogLines;
	// Use this for initialization
	void Awake () {
        dialogLines = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addLine(string line)
    {
        dialogLines.Add(line);
    }

    public string getLine(int index)
    {
        return dialogLines[index];
    }

    public abstract void exitTalking();
}
