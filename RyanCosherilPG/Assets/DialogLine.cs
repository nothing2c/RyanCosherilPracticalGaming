using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogLine {

    string line;
    public List<string> responses;
    public enum effect {continueDialog, exitDialog, attack};
    effect lineEffect;
    bool needsResponse;

    public DialogLine(string line, bool needsResponse , effect lineEffect)
    {
        this.line = line;
        this.needsResponse = needsResponse;
        this.lineEffect = lineEffect;
        responses = new List<string>();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string getLine()
    {
        return line;
    }

    public void addResponse(string response)
    {
        responses.Add(response);
    }
}
