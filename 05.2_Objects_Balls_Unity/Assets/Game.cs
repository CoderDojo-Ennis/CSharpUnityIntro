using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject ballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //makeBall(1, 2, Color.red);
        makeBalls(20);
    }

    void makeBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(0f, 20f);
            float y = Random.Range(0f, 20f);
            Color color = Color.red;
            makeBall(x, y, color);
        }
    }

    void makeBall(float x, float y, Color color)
    {
        GameObject ball = Instantiate(ballPrefab);
        BallMoveScript ballMover = ball.GetComponent<BallMoveScript>();
        ball.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
