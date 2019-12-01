using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Color;

public class ColorChange : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isPast;

    private Color originalColor;

    private void Start()
    {
        originalColor = transform.GetComponent<Renderer>().material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigered");
        ChairTracker chair = other.GetComponentInChildren<ChairTracker>();

        if (isPast)
        {
            chair.enterPast();
        }
        else
        {
            chair.enterFuture();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        transform.GetComponent<Renderer>().material.SetColor("_Color", originalColor);
        ChairTracker chair = other.GetComponentInChildren<ChairTracker>();
        if (isPast)
        {
            chair.exitPast();
        }
        else
        {
            chair.exitFuture();
        }
    }
}