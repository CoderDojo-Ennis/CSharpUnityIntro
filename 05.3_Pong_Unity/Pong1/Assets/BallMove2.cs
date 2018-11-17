using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove2 : MonoBehaviour
{
    public float Speed = 10;
    public float RandomBounce = 5;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(3, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.velocity.normalized * Speed;
    }

    public void BounceRandom()
    {
        float addX = Random.Range(-RandomBounce, RandomBounce);
        float addY = Random.Range(-RandomBounce, RandomBounce);
        rb.velocity = rb.velocity + new Vector3(addX, addY, 0);
    }

    public void OnCollisionEnter(Collision collision)
    {
        BounceRandom();
    }
}
