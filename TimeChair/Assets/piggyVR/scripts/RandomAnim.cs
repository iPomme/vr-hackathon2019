using System;
using System.Collections;
using System.Collections.Generic;
using Akka.Actor;
using Jorand.AkkaUnity;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAnim : MonoBehaviourActor
{
    public int rotateDelayInMilliseconds = 300;
    public int forwardDelayInMilliseconds = 400;
    public int jumpDelayInMilliseconds = 100;

    public Vector2 minXZ = new Vector2(-3, -4);
    public Vector2 maxXZ = new Vector2(7, 5);
    public float maxHeight = 4f;

    private Rigidbody rigid;

    // Start is called before the first frame update

    private void Start()
    {
        base.Start(name);
        rigid = gameObject.GetComponent<Rigidbody>();
        system.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
            TimeSpan.FromMilliseconds(rotateDelayInMilliseconds),
            internalActor, new RotateMsg(), ActorRefs.NoSender);
        system.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
            TimeSpan.FromMilliseconds(forwardDelayInMilliseconds),
            internalActor, new MoveMsg(), ActorRefs.NoSender);
        system.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
            TimeSpan.FromMilliseconds(jumpDelayInMilliseconds),
            internalActor, new Jump(), ActorRefs.NoSender);
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentRotation = transform.eulerAngles;


        transform.eulerAngles = currentRotation;

        Vector3 rVel = rigid.velocity;
        if (transform.position.x > maxXZ.x)
        {
            rVel = new Vector3(0, rVel.y, rVel.z);
            currentPosition.x = maxXZ.x;
        }

        if (transform.position.x < minXZ.x)
        {
            rVel = new Vector3(0, rVel.y, rVel.z);
            currentPosition.x = minXZ.x;
        }

        if (transform.position.z > maxXZ.y)
        {
            rVel = new Vector3(rVel.x, rVel.y, 0);
            currentPosition.z = maxXZ.y;
        }

        if (transform.position.z < minXZ.y)
        {
            rVel = new Vector3(rVel.x, rVel.y, 0);
            currentPosition.z = minXZ.y;
        }

        if (transform.position.y > maxHeight)
        {
            rVel = new Vector3(rVel.x, 0, rVel.z);
            currentPosition.y = 1.5f;
        }

        rigid.velocity = new Vector3(rVel.x, rVel.y, rVel.z);
        transform.position = currentPosition;
    }

    // Update is called once per frame
    private void rotate()
    {
        transform.Rotate(0, 0, Random.Range(-90, 90), Space.Self);
    }

    private void addForce(float thrust)
    {
        rigid.AddForce(-transform.up * thrust * 10, ForceMode.Impulse);
    }

    private void jump(float thrust)
    {
        rigid.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }

    public override void OnReceive(object message, IActorRef sender)
    {
        switch (message)
        {
            case RotateMsg _:
                UnityThread.executeInUpdate(() => rotate());
                break;
            case MoveMsg _:
                UnityThread.executeInUpdate(() => addForce(Random.Range(0f, 1f)));
                break;
            case Jump _:
                UnityThread.executeInUpdate(() => jump(Random.Range(0f, 1f)));
                break;
        }
    }
}

public class RotateMsg
{
}

public class MoveMsg
{
}

public class Jump
{
}