using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spartan : NPC, Interactable {

    Animator animator;
    enum States { idle, chatting, aggro};
    States currentState;
    GameObject player;
    // Use this for initialization
    void Start () {
        characterName = "Spartan";

        animator = gameObject.GetComponent<Animator>();
        currentState = States.idle;
        characterName = "Andre";
        currentLineIndex = 0;
        addLine(new DialogLine("Im only here because i came with a death animation", false, DialogLine.effect.continueDialog));
        addLine(new DialogLine("Im also here to test the attack effect of dialog", false, DialogLine.effect.continueDialog));
        addLine(new DialogLine("So im going to attack you now, ignore my hand its a feature", false, DialogLine.effect.continueDialog));
        addLine(new DialogLine("I couldnt get it working and need to study other things", false, DialogLine.effect.attack));
    }
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case States.idle:
                
                break;
            case States.chatting:
                animator.SetBool("StartTalking", false);
                break;
            case States.aggro:
                break;
        }
    }

    public override void exitTalking()
    {
        currentLineIndex = 0;
        currentState = States.aggro;
        animator.SetBool("IsAttacking", true);
    }

    public void interact(GameObject interactor)
    {
        currentState = States.chatting;
        animator.SetBool("StartTalking", true);
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
}
