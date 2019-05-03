using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour {

    public string characterName;
    List<DialogLine> dialogLines;
    public int currentLineIndex;
	// Use this for initialization
	void Awake () {
        dialogLines = new List<DialogLine>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addLine(DialogLine line)
    {
        dialogLines.Add(line);
    }

    public DialogLine getLine(int index)
    {
        return dialogLines[index];
    }

    public abstract void exitTalking();
}
