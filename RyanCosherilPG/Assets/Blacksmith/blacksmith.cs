using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blacksmith : NPC , Interactable {

    Animator animator;
    enum States {idle, chatting};
    States currentState;
    GameObject player;

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator>();
        currentState = States.idle;
        characterName = "Andre";
        addLine("My face hurts");
	}
	
	// Update is called once per frame
	void Update () {
		switch(currentState)
        {
            case States.idle:
                animator.SetBool("IsTalking", false);
                break;
            case States.chatting:
                animator.SetBool("IsTalking", true);
                break;
        }
	}

    public void interact(GameObject interactor)
    {
        currentState = States.chatting;
        GameManager.talkingNPC = this;
        FindObjectOfType<GameManager>().setCurrentGameState(GameManager.GameStates.talking);
        player = interactor;
    }

    public bool isInteractable()
    {
        if (currentState == States.idle)
            return true;
        else
            return false;
    }

    public override void exitTalking()
    {
        currentState = States.idle;
    }
}
