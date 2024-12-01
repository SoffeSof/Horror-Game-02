using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light lightSource;
    public Light otherLight;
    public float minTime;
    public float maxTime;
    private float timer;

    private void Start()
    {
        lightSource = GetComponent<Light>();
        timer = Random.Range(minTime, maxTime);
    }

    private void Update()
    {
        Flickering();
    }

    private void Flickering()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            lightSource.enabled = !lightSource.enabled;
            if (otherLight != null)
            {   
                otherLight.enabled = !otherLight.enabled;
            }
            timer = Random.Range(minTime, maxTime);
        }
    }
}