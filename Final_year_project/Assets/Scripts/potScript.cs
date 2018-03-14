using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potScript : MonoBehaviour {

    GMScript gm;
    turnManagerScript tm;
    playerManager pm;
    Rigidbody rb;
    GameObject colObject;

    private void Start()
    {
        gm = GMScript.gameMan;
        tm = turnManagerScript.turnManager;
        pm = playerManager.playerMan;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        colObject = collision.gameObject;
        turnManagerScript.turnManager.AddBallToPottedList(colObject);

        Debug.Log("Ball potted: " + colObject.name);
        if(tag == "spotBall" || tag == "stripeBall" || tag == "cueBall" || tag == "blackBall")
        {
            gm.RemoveBallFromList(colObject);
            colObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        string tag = other.gameObject.tag;
        colObject = other.gameObject;
        turnManagerScript.turnManager.AddBallToPottedList(colObject);

        if (tag == "spotBall" || tag == "stripeBall" || tag == "cueBall" || tag == "blackBall")
        {
            gm.RemoveBallFromList(colObject);
            colObject.SetActive(false);

            if (tag == "spotBall")
            {
                //GMScript.gameMan.PottedSpotBall();
            }
            else if(tag == "stripeBall") {
                //GMScript.gameMan.PottedStripeBall();
            }
        }
    }
}
