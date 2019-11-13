using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAnim : MonoBehaviour
{

    private Rigidbody rigid;
    
    // Start is called before the first frame update

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the game object that this script is attached to by 15 in the X axis,
        // 30 in the Y axis and 45 in the Z axis, multiplied by deltaTime in order to make it per second
        // rather than per frame.
        transform.Rotate (new Vector3 (0, Random.Range(-45,45), 0) * Time.deltaTime);

//        rigid.velocity = transform.TransformDirection(Vector3.forward*2f);


    }
}
