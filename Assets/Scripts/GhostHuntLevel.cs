using UnityEngine;

public class GhostHuntLevel : MonoBehaviour
{
  public enum State
  {
    DialogueStart,
    GhostHunt,
    DialogueFinish,
  }

  public Dialogue dialogue;
  public State state = State.DialogueStart;
  public Ghost ghost;

  // Start is called before the first frame update
  void Start()
  {
    dialogue.waitingForEvent.AddListener(OnNextState);
    ghost.onFinish.AddListener(OnNextState);
  }

  void OnNextState()
  {
    if (state == State.DialogueStart)
    {
      ghost.started = true;
      state = State.GhostHunt;
      dialogue.Show(false);
      return;
    }
    if (state == State.GhostHunt)
    {
      dialogue.Interact();
      dialogue.Show(true);
      state = State.DialogueFinish;
      return;
    }
    if (state == State.DialogueFinish)
    {
      Debug.Log("level 3 time");
      return;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
