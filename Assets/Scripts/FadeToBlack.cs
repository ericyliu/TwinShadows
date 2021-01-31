using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class FadeToBlack : MonoBehaviour
{

    public enum Type
    {
        ToBlack,
        FromBlack
    }

    private Image image;

    public float originalAlpha = 0;
    public float targetAlpha = 0;
    public float t = 1;

    public bool isFading = false;

    public UnityEvent finishedFadingToBlack = new UnityEvent();
    public UnityEvent finishedFadingFromBlack = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFading)
            return;

        float a = 0;

        if (t < 1) {
            a = Mathf.Lerp(originalAlpha, targetAlpha, t);
            t += Time.deltaTime;
        }
        else {
            t = 1;
            a = targetAlpha;
            isFading = false;

            if (targetAlpha == 1) {
                finishedFadingToBlack.Invoke();
            }
            else {
                image.enabled = false;
                finishedFadingFromBlack.Invoke();
            }
        }

        image.color = new Color(0, 0, 0, a);
    }

    public void Fade(Type type) {
        image.enabled = true;
        originalAlpha = (type == Type.ToBlack) ? 0 : 1;
        targetAlpha = (type == Type.ToBlack) ? 1 : 0;
        t = 0;
        isFading = true;
    }
}
