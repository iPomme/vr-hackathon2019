using System;
using System.Text;
using Akka.Actor;
using Akka.IO;
using Akka.Util.Internal;
using DotNetty.Common.Utilities;
using Jorand.AkkaUnity;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using Random = System.Random;

public class WebSocketUnityServer : MonoBehaviourActor
{

    public int port = 8989;

    public GameObject chairTracker;
    
    // Start is called before the first frame update
    private WebSocketServer wssv;
    private string temperature;
    private Echo EcoAndler;
    private void Start()
    {
        UnityThread.initUnityThread();
    }

    public void sendtochair(string msg)
    {
        EcoAndler.sendto(msg);
    }
    private void OnEnable()
    {

        
        wssv = new WebSocketServer(port);
        EcoAndler = new Echo(chairTracker);
        wssv.AddWebSocketService<Echo>("/chair",() => EcoAndler);
        
        wssv.Start();
        
        Debug.LogFormat("Websocket Server started on port {0}", port);
    }

    public override void OnReceive(object message, IActorRef sender)
    {
        throw new NotImplementedException();
    }
    
    void Foo(){}

    private void Update()
    {
        temperature = UnityEngine.Random.Range(0, 100).ToString();
    }
}


public class Echo : WebSocketBehavior
{
    private int i = 0;
    private ChairTracker _tracker;
    
    public Echo (GameObject tracker)
    {
        _tracker = tracker.GetComponent<ChairTracker>();
    }

    public void sendto(string msg)
    {
        Send(msg);
    }
    protected override void OnMessage(MessageEventArgs e)
    {
        var buffer = e.RawData;
//        Debug.LogFormat("Got message from the websocket '{0}'", BitConverter.ToString(buffer));

//        Debug.LogFormat("Got message from the websocket solar value'{0}'", BitConverter.ToInt16(buffer,0));
        int assis = BitConverter.ToInt16(buffer, 2);
        //Debug.LogFormat("Got message from the websocket assis? '{0}'", assis);
        _tracker.assis(assis);
       // Debug.LogFormat("Got message from the websocket temperature? '{0}'", BitConverter.ToInt16(buffer,4));

        if (i % 100 == 0)
        {
            Random r = new Random();
            int rInt = r.Next(0, 100);
            string temperature = rInt.ToString();
            Debug.LogFormat("Message '{0}' sent back to the client", temperature);
            Send(temperature); 
                                   }
        i++;
    }
}