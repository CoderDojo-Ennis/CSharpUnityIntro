using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveScript : MonoBehaviour
{
    public float xSpeed = 0.1f;
    public float ySpeed = 0.1f;
    public float x = 0;
    public float y = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Draw();
    }

    private void Move()
    {
        x += xSpeed;
        y += ySpeed;
        BounceOnEdge();
    }

    private void Draw()
    {
        transform.position = new Vector3(x, y);
    }

    void BounceOnEdge()
    {
        if (x > 6 || x < -6)
        {
            xSpeed *= -1;
        }

        if (y > 4.5 || y < -4.5)
        {
            ySpeed *= -1;
        }
    }
}
