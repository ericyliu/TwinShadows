using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

  public Dialogue dia;
  public FadeToBlack fader;

  public Transform InFrontOfSonsRoom;
  public Transform InFrontOfStairs;
  public Transform BottomOfStairs;
  public Transform InFrontOfDoor;


  // Start is called before the first frame update
  void Start()
  {
    dia.waitingForEvent.AddListener(OnDialogueWaitingForEvent);
    fader.finishedFadingToBlack.AddListener(OnFadedToBlack);
    fader.finishedFadingFromBlack.AddListener(OnFadedFromBlack);
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnDialogueWaitingForEvent()
  {
    fader.Fade(FadeToBlack.Type.ToBlack);
    dia.EnableButton(false);
  }

  void OnFadedToBlack()
  {

    // Fade back in unless done
    if (state != State.Leaving)
      fader.Fade(FadeToBlack.Type.FromBlack);

    dia.Show(false);

    switch (state)
    {
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
        SceneManager.LoadScene("GhostHunt");
        break;
    }
  }

  void OnFadedFromBlack()
  {
    dia.Interact();
    dia.Show(true);
    dia.EnableButton(true);
    }

}
