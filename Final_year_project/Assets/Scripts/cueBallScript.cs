﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cueBallScript : MonoBehaviour {
    playerManager pm;
    GMScript gm;
    turnManagerScript tm;

	// Use this for initialization
	void Start () {
        pm = playerManager.playerMan;
        gm = GMScript.gameMan;
        tm = turnManagerScript.turnManager;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if(tm == null)
        {
            tm = turnManagerScript.turnManager;
        }
        tm.SetFirstBallHitThisTurn(collision.gameObject);
    }
}
