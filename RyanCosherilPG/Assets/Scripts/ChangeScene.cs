﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void changeScene(Scene sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad.buildIndex);
    }
}
