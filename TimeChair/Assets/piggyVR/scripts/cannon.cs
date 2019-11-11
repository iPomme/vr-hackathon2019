using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannon : MonoBehaviour
{
    public Rigidbody goldcoin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
        {
            Rigidbody newcoin;
            newcoin = Instantiate(goldcoin, transform.position, transform.rotation);
            newcoin.velocity = transform.TransformDirection(Vector3.forward * 10);

        }
    }
    
}
