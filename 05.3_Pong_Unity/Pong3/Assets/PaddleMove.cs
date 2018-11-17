using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMove : MonoBehaviour
{
    public float Speed = 3;
    public float MinY = -5;
    public float MaxY = 5;
    public int PlayerNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float dy = GetInput()
            * Speed * Time.deltaTime;
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

    private float GetInput()
    {
        if (PlayerNumber == 0)
        {
            return Input.GetAxisRaw("Vertical");
        }
        if (PlayerNumber == 1)
        {
            return Input.GetAxisRaw("Vertcial2");
        }

        return 0;
    }

}
