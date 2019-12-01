using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairTracker : MonoBehaviour
{
    public GameObject chair;


    private float initialHeight;

    private float deltaUp = .20f;

    private float deltaBack = .05f;

    private bool chairDown = true;

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            Debug.LogFormat("Chair Setup at '{0}'", transform.position.y);

            initialHeight = chair.transform.position.y;
            chairDown = true;
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.LogFormat("Current Position '{0}'", transform.position);
        }

        if (initialHeight != null && transform.position.y > initialHeight + deltaUp && chairDown)
        {
            Debug.Log("Chair UP");
            chairDown = false;
        }

        if (initialHeight != null && transform.position.y < initialHeight + deltaBack && !chairDown)
        {
            Debug.Log("Chair Down");
        }
    }

    public void enterPast()
    {
        Debug.Log("Entered in the past");
    }

    public void exitPast()
    {
        Debug.Log("Exit in the past");
    }

    public void enterFuture()
    {
        Debug.Log("Entered in the future");
    }

    public void exitFuture()
    {
        Debug.Log("Exit in the future");
    }
}