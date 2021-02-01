using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonsHouseLevel : MonoBehaviour
{

    public enum State
    {
        EnterHouse,
        WalkToDen,
        HoldingPhoto,
        WatchingTV,
        End,
    }
    
    public State state;

    public Dialogue dia;
    public FadeToBlack fader;

    public Transform IntoKitchen;
    public Transform HoldingPhoto;
    public Transform FacingTV;
    public Transform LastGlancePOV;


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

    void OnDialogueWaitingForEvent() {
        fader.Fade(FadeToBlack.Type.ToBlack);
        dia.EnableButton(false);
    }

    void OnFadedToBlack() {
        // Fade back in unless done
        if (state != State.End)
            fader.Fade(FadeToBlack.Type.FromBlack);

        switch (state) {
            case State.EnterHouse:
                transform.position = IntoKitchen.transform.position;
                transform.rotation = IntoKitchen.transform.rotation;
                state = State.WalkToDen;
                break;

            case State.WalkToDen:
                transform.position = HoldingPhoto.transform.position;
                transform.rotation = HoldingPhoto.transform.rotation;
                state = State.HoldingPhoto;
                break;

            case State.HoldingPhoto:
                transform.position = FacingTV.transform.position;
                transform.rotation = FacingTV.transform.rotation;
                state = State.WatchingTV;
                break;

            case State.WatchingTV:
                transform.position = LastGlancePOV.transform.position;
                transform.rotation = LastGlancePOV.transform.rotation;
                state = State.End;
                break;

            case State.End:
                break;
        }
    }

    void OnFadedFromBlack() {
        dia.EnableButton(true);
        dia.Interact();
    }
}
