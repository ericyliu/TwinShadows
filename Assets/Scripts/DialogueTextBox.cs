using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextBox : MonoBehaviour
{

    public Dialogue dia;

    // false = IsDialogue
    public bool IsSpeakerName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSpeakerName) 
            GetComponent<Text>().text = System.Enum.GetName(typeof(Dialogue.Speaker), dia.pendingLine.speaker);
        else
            GetComponent<Text>().text = dia.Say();
    }
}
