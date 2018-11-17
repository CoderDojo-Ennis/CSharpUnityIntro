using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public Vector2 Direction = new Vector2(1,0);
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.Direction.Normalize();
        this.transform.Translate(Direction * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector2 distanceToOther = other.ClosestPoint(this.transform.position) - this.transform.position;
        Debug.Log(distanceToOther);

        if (distanceToOther.x < -.2f || distanceToOther.x > .2f)
        {
            this.Direction.x = this.Direction.x * -1f;
        }
        if (distanceToOther.y < -.2f || distanceToOther.y > .2f)
        {
            this.Direction.y = this.Direction.y * -1f;
        }

    }
}
