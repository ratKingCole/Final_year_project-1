using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{

    GameObject ball;
    GameObject cues;
    Vector3 cameraOffset;

    void Start()
    {

    }

    void Update()
    {
        try
        {
            ball = cueBallScript.cueBallSingleton.gameObject;
        } catch
        {
            Debug.Log("cueBall singleton not found. Maybe object is not spawned yet?");
        }
        cues = GameObject.Find("poolCue");

        string cameName = this.GetComponent<cam_switcher>().currentCamera;
        bool moving = this.GetComponent<cam_switcher>().moving;
        if (!moving)
        {
            if (cameName == "firstperson")
            {
                transform.parent = null;
                cameraOffset = new Vector3(0f, 0f, 0f);
                transform.position = new Vector3(ball.transform.position.x + cameraOffset.x, ball.transform.position.y + cameraOffset.y, ball.transform.position.z + cameraOffset.z);
            }
           else if (cameName == "thirdperson")
            {
                transform.parent = null;
                cameraOffset = new Vector3(0f, 5f, -5f);
                transform.position = new Vector3(ball.transform.position.x + cameraOffset.x, ball.transform.position.y + cameraOffset.y, ball.transform.position.z + cameraOffset.z);

            }
            else if (cameName == "cue")
            {
                cues = GameObject.Find("poolCue");
                if (cues != null)
                {
                    transform.SetParent(cues.transform);
                    cameraOffset = new Vector3(0f, 5f, 0f);
                    transform.position = new Vector3(cues.transform.position.x + cameraOffset.x, cues.transform.position.y + cameraOffset.y, cues.transform.position.z + cameraOffset.z);
                }

            }
            else if(cameName == "overhead")
            {
                transform.parent = null;
                transform.position = transform.position;
            }
            else {
                transform.parent = null;
                transform.position = transform.position;
            }
        }

    }
}