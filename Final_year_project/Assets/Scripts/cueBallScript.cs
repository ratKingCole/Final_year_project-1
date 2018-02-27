using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cueBallScript : MonoBehaviour {
    playerManager pm;
    GMScript gm;
    turnManagerScript tm;
    GameObject cueBall;

	// Use this for initialization
	void Start () {
        pm = playerManager.playerMan;
        gm = GMScript.gameMan;
        tm = turnManagerScript.turnManager;
        cueBall = gm.GetCueBall();
    }
	
	// Update is called once per frame
	void Update () {
        //GetComponent<Rigidbody>().AddRelativeTorque(0, 0, 30); left and right
        //GetComponent<Rigidbody>().AddTorque(30, 0, 0); up and down
    }

    void OnCollisionEnter(Collision collision)
    {
        if(tm == null)
        {
            tm = turnManagerScript.turnManager;
        }
        tm.SetFirstBallHitThisTurn(collision.gameObject);

        if(collision != null)
        {
            cueBall.GetComponent<ConstantForce>().torque = Vector3.zero;
            poolCue.spin = false;
        }
    }
}
