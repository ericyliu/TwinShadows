using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVFlicker : MonoBehaviour
{
    float initialIntensity;

    // Start is called before the first frame update
    void Start()
    {
        initialIntensity = GetComponent<Light>().intensity;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Light>().intensity = initialIntensity + 10 * Mathf.PerlinNoise(1, 10 * Time.time);
    }
}
