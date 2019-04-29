using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public enum GameScene { Hub, Forrest}
    public GameScene targetScene;
    GameManager manager;
	// Use this for initialization
	void Start () {
        manager = FindObjectOfType<GameManager>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {  
            manager.changeScene((int) targetScene);
        }
            
    }
}
