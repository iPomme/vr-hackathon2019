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

    public float deltaUp = .20f;
    public float deltaBack = .05f;
    
    
    private Animator _animator;
    private float initialHeight;

    private const string PAST = "PAST";
    private const string PRESENT = "PRESENT";
    private const string FUTURE = "FUTURE";

    private string currentState = PRESENT;
    
    private bool chairDown = true;

    private void Start()
    {
        _animator = chair.GetComponent<Animator>();
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
                break;
            case FUTURE:
                Debug.LogFormat("Assis({0}) in the future", val);
                break;
            case PAST:
                Debug.LogFormat("Assis({0}) in the past", val);
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