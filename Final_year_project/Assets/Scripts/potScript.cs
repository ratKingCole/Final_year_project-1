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
        if (!sim.simulator.get_sim_active())
        {
            tm.AddBallToPottedList(colObject);

            if (tag == "spotBall" || tag == "stripeBall" || tag == "cueBall" || tag == "blackBall")
            {
                gm.RemoveBallFromList(colObject);
                colObject.SetActive(false);
            }
        }
        else Destroy(colObject);
    }
}
