using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{

    public enum State
    {
        DialogueWakingUp,
        DialogueSonsRoom,
        DialogueTopOfStairs,
        DialogueDownstairs,
        DialogueFrontdoor,
        Leaving
    }
    
    public State state;

    public SplineWalker cam;
    public Dialogue dia;
    public FadeToBlack fader;

    public Transform InFrontOfSonsRoom;
    public Transform InFrontOfStairs;
    public Transform BottomOfStairs;
    public Transform InFrontOfDoor;

    bool willTP = false;
    

    // Start is called before the first frame update
    void Start()
    {
        dia.continueButton.onClick.AddListener(OnContinueDialogue);
        dia.waitingForEvent.AddListener(OnDialogueWaitingForEvent);
        fader.finishedFadingToBlack.AddListener(OnFadedToBlack);
        fader.finishedFadingFromBlack.AddListener(OnFadedFromBlack);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDialogueWaitingForEvent() {
        fader.Fade(FadeToBlack.Type.ToBlack);
    }

    void OnFadedToBlack() {

        // Fade back in unless done
        if (state != State.Leaving)
            fader.Fade(FadeToBlack.Type.FromBlack);

        switch (state) {
            case State.DialogueWakingUp:
                print("go to sons rom");
                transform.position = InFrontOfSonsRoom.transform.position;
                transform.rotation = InFrontOfSonsRoom.transform.rotation;
                state = State.DialogueSonsRoom;
                break;

            case State.DialogueSonsRoom:
                print("go to stairs");
                transform.position = InFrontOfStairs.transform.position;
                transform.rotation = InFrontOfStairs.transform.rotation;
                state = State.DialogueTopOfStairs;
                break;

            case State.DialogueTopOfStairs:
                print("go downstairs");
                transform.position = BottomOfStairs.transform.position;
                transform.rotation = BottomOfStairs.transform.rotation;
                state = State.DialogueDownstairs;
                break;

            case State.DialogueDownstairs:
                print("go to door");
                transform.position = InFrontOfDoor.transform.position;
                transform.rotation = InFrontOfDoor.transform.rotation;
                state = State.Leaving;
                break;


            case State.Leaving:
                print("GO OUTSIDE");
                break;
        }
    }

    void OnFadedFromBlack() {
        dia.Interact();
    }

    void OnContinueDialogue() {
        return;

        switch (state) {
            // "couldn't sleep again"
            // start getting out of bed when dialogue starts
            case State.DialogueWakingUp:
                fader.Fade(FadeToBlack.Type.ToBlack);
                break;

            // "his room"
            case State.DialogueSonsRoom:
                break;

            // "can't sleep. don't want coffee"
            case State.DialogueDownstairs:
                break;

            // "maybe i should go out again"
            case State.Leaving:
                break;
        }
    }
}
