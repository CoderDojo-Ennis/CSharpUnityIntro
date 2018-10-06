using UnityEngine;
using GeekyMonkey;

public class BallMoveScript : MonoBehaviour
{
    public int Speed = -1;
    public int X = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.Forever(1, MoveBall);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void MoveBall()
    {
        X = X + Speed;
        transform.position = new Vector3(X, 0, 0);
    }
}
