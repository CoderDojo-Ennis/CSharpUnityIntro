using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject Ball;
    public float MinY = -3.6f;
    public float MaxY = 3.6f;
    public float VisibleDistance = 4f;
    public float Speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToBall = (Ball.transform.position - this.transform.position).magnitude;
        if (distanceToBall < VisibleDistance)
        {
            float direction = 0;
            if (Ball.transform.position.y - this.transform.position.y > 1)
            {
                direction = 1;
            } else if (Ball.transform.position.y - this.transform.position.y < -1)
            {
                direction = -1;
            }

            float dy = direction * Speed * Time.deltaTime;
            transform.Translate(0, dy, 0);

            if (transform.position.y < MinY)
            {
                transform.position = new Vector3(transform.position.x, MinY, 0);
            }
            if (transform.position.y > MaxY)
            {
                transform.position = new Vector3(transform.position.x, MaxY, 0);
            }
        }
    }
}
