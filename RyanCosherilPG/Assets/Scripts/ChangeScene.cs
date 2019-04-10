using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public enum GameScene { Hub, Forrest}
    public GameScene targetScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("changed");
            SceneManager.LoadScene((int) targetScene);
        }
            
    }

    public static void changeScene(Scene sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad.buildIndex);
    }
}
