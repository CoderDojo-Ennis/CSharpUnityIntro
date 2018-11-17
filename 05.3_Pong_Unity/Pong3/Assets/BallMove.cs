using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Rigidbody rb;

    public float Speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(5, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.velocity.normalized * Speed;

        rb.angularVelocity = Vector3.right * .5f;
    }
}
