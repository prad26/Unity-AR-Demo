using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public List<GameObject> rotate;

    float speed  = 20.0f;
    void Update()
    {
        rotate[0].transform.Rotate(0,0,speed);
        rotate[1].transform.Rotate(-speed,0,0);
    }
}
