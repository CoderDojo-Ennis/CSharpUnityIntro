using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject ballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        makeBall(1, 2, Color.red);
        makeBalls(20);
    }

    void makeBall(float x, float y, Color color)
    {
        GameObject ball = Instantiate(ballPrefab);
        BallMoveScript ballMover = ball.GetComponent<BallMoveScript>();
        ballMover.x = x;
        ballMover.y = y;
        ballMover.xSpeed = Random.Range(0.01f, .3f);
        ballMover.ySpeed = Random.Range(0.01f, .3f);

        ball.GetComponent<Renderer>().material.color = color;
    }

    void makeBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            makeBall(Random.Range(-6f, 6f), Random.Range(-4.5f, 4.5f), new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
