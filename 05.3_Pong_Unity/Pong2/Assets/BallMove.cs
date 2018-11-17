using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Rigidbody rb;
    public float Speed = 8;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(3, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        SetBallSpeed();
    }

    void SetBallSpeed()
    {
        rb.velocity = rb.velocity.normalized * Speed;
        transform.Rotate(rb.velocity);
    }
}
