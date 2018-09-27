using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveScript : MonoBehaviour
{
    public float Speed = 1;
    public int X = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        X = X + (int)Speed;
        transform.position = new Vector3(X, 0, 0);
    }
}
