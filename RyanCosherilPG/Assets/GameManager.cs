using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static List<Enemy> targets;
    public Canvas HUD;
    public Image screenTransition;
    public DialogBox dialogBox;
    public enum GameStates {freeRoam, talking, playerDead}
    public static GameStates currentGameState;
    public static NPC talkingNPC;
    public GameObject player;
    bool needToChangeScene;

    // Use this for initialization
    void Awake () {
        targets = new List<Enemy>();
        StartCoroutine(fadeIn());
        dialogBox.gameObject.SetActive(false);
        currentGameState = GameStates.freeRoam;
        player = FindObjectOfType<PlayerControl>().gameObject;
    }
	
	// Update is called once per frame
	void Update () {    
        switch(currentGameState)
        {
            case GameStates.freeRoam:
                dialogBox.gameObject.SetActive(false);
                break;
            case GameStates.talking:
                dialogBox.characterName.text = talkingNPC.characterName;
                dialogBox.dialog.text = talkingNPC.getLine(0);
                dialogBox.gameObject.SetActive(true);

                if (Vector3.Distance(player.transform.position, talkingNPC.transform.position) > 5)
                {
                    talkingNPC.exitTalking();
                    currentGameState = GameStates.freeRoam;
                }
                break;
            case GameStates.playerDead:
                GameObject newGameObject = new GameObject();
                newGameObject.transform.SetParent(HUD.transform);
                Text youDied = newGameObject.AddComponent<Text>();
                youDied.text = "You Died";
                youDied.color = Color.red;
                youDied.fontSize = 100;
                youDied.font = Resources.Load<Font>("brotherhood");
                youDied.alignment = TextAnchor.MiddleCenter;

                changeScene(SceneManager.GetActiveScene().buildIndex);
                currentGameState = GameStates.freeRoam;
                break;
        }
            
    }

    public void changeScene(int sceneIndex)
    {
        needToChangeScene = true;
        StartCoroutine(fadeOut(sceneIndex));
    }

    IEnumerator fadeIn()
    {
        Color c = screenTransition.color;
        while (screenTransition.color.a > 0)
        {
            c.a -= Time.deltaTime / 2;
            screenTransition.color = c;
            yield return null;
        }
    }

    IEnumerator fadeOut(int sceneIndex)
    {
        Color c = screenTransition.color;
        while (screenTransition.color.a < 1)
        {
            c.a += Time.deltaTime / 1;
            screenTransition.color = c;

            yield return null;
        }

        if (needToChangeScene)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        yield return null;
    }
}
