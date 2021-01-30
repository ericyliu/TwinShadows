using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueNextButton : MonoBehaviour
{

    public Dialogue dia;

    // Start is called before the first frame update
    void Start() {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick() {
        dia.Interact();
    }
}
