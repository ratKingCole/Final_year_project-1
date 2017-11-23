using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour {

    GameObject ball;
    Vector3 cameraOffset;

    void Start()
    {

    }

    void Update()
    {
        ball = GameObject.Find("cueBall(Clone)");
        cameraOffset = new Vector3(0f, 5f, -5f);
        if(gameObject.name == "firstperson") cameraOffset = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(ball.transform.position.x + cameraOffset.x, ball.transform.position.y + cameraOffset.y, ball.transform.position.z + cameraOffset.z);
    }
}