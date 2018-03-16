using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseColourScript : MonoBehaviour {

    turnManagerScript tm;
    playerManager pm;

	// Use this for initialization
	void Start () {
        tm = turnManagerScript.turnManager;
        pm = playerManager.playerMan;
	}
	
	public void ChooseSpots()
    {
        SetColour(GMScript.Target.Spots);
    }

    public void ChooseStripes()
    {
        SetColour(GMScript.Target.Stripes);
    }

    void SetColour(GMScript.Target _target)
    {
        bool isPlayer1Turn = tm.GetIsPlayer1Turn();
        GMScript.Target _otherPlayerTarget;
        if(_target == GMScript.Target.Spots)
        {
            _otherPlayerTarget = GMScript.Target.Stripes;
        } else
        {
            _otherPlayerTarget = GMScript.Target.Spots;
        }

        if(isPlayer1Turn)
        {
            pm.SetPlayer1Target(_target);
            pm.SetPlayer2Target(_otherPlayerTarget);
        } else
        {
            pm.SetPlayer2Target(_target);
            pm.SetPlayer1Target(_otherPlayerTarget);
        }
        tm.PlayerHasChosenColour();
    }
}
