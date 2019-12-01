using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairTracker : MonoBehaviour
{
    public GameObject chair;


    private float initialHeight;

    private float deltaUp = 20f;

    private float deltaBack = 5f;

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            initialHeight = chair.transform.position.y;
        }

        if (initialHeight != null && initialHeight > initialHeight + deltaUp)
        {
            Debug.Log("Chair UP");
        }

        if (initialHeight != null && initialHeight < initialHeight + deltaBack)
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