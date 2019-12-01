using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChairTracker : MonoBehaviour
{
    public GameObject chair;
    public GameObject pastEnvironment;
    public GameObject futureEnvironment;
    public GameObject websoket;

    public float deltaUp = .20f;
    public float deltaBack = .05f;
    
    
    private Animator _animator;
    private float initialHeight;

    private const string PAST = "PAST";
    private const string PRESENT = "PRESENT";
    private const string FUTURE = "FUTURE";

    private string currentState = PRESENT;
    
    private bool chairDown = true;
    private WebSocketUnityServer _server;

    private void Start()
    {
        _animator = chair.GetComponent<Animator>();
        _server = websoket.GetComponent<WebSocketUnityServer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            Debug.LogFormat("Chair Setup at '{0}'", transform.position.y);

            initialHeight = chair.transform.position.y;
            chairDown = true;
            _animator.SetTrigger("neutral");
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.LogFormat("Current Position '{0}'", transform.position);
        }

        if (initialHeight != null && transform.position.y > initialHeight + deltaUp && chairDown)
        {
            Debug.Log("Chair UP");
            chairDown = false;
            switch (currentState)
            {
                case PRESENT:
                    _animator.SetTrigger("neutral_transport");
                    break;
                case FUTURE:
                    _animator.SetTrigger("neutral_transport");
                    break;
                case PAST:
                    _animator.SetTrigger("neutral_transport");
                    break;
            }
        }

        if (initialHeight != null && transform.position.y < initialHeight + deltaBack && !chairDown)
        {
            Debug.Log("Chair Down");
            chairDown = true;
            switch (currentState)
            {
            case PRESENT:
                _animator.SetTrigger("present");
                break;
            case FUTURE:
                _animator.SetTrigger("future");
                break;
            case PAST:
                _animator.SetTrigger("past");
                break;
            }
        }
    }

    public void assis(int val)
    {
        switch (currentState)
        {
            case PRESENT:
                Debug.LogFormat("Assis({0}) in the present", val);
                _server.sendtochair("20");
                Debug.Log("send(20) to the soket");

                
                break;
            case FUTURE:
                Debug.LogFormat("Assis({0}) in the future", val);
                _server.sendtochair("60");
                Debug.Log("send(60) to the soket");


                break;
            case PAST:
                Debug.LogFormat("Assis({0}) in the past", val);
                _server.sendtochair("0");
                Debug.Log("send(0) to the soket");

                break;
        }
    }
    public void enterPast()
    {
        Debug.Log("Entered in the past");
        currentState = PAST;
        _animator.SetTrigger("past");
    }

    public void exitPast()
    {
        Debug.Log("Exit in the past");
        currentState = PRESENT;
        _animator.SetTrigger("present");
    }

    public void enterFuture()
    {
        Debug.Log("Entered in the future");
        currentState = FUTURE;
        _animator.SetTrigger("future");
    }

    public void exitFuture()
    {
        Debug.Log("Exit in the future");
        currentState = PRESENT;
        _animator.SetTrigger("present");
    }
}