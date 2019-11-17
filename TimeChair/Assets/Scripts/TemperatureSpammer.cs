using System;
using System.Collections;
using System.Collections.Generic;
using Akka.Actor;
using Jorand.AkkaUnity;
using UnityEngine;

public class TemperatureSpammer : MonoBehaviourActor
{
    // Start is called before the first frame update
    void Start()
    {
        system.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromMilliseconds(0),TimeSpan.FromMilliseconds(500),internalActor,"Spam", internalActor );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnReceive(object message, IActorRef sender)
    {
        
    }
}
