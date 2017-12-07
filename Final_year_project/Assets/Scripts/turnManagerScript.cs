using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManagerScript : MonoBehaviour {

    public static turnManagerScript turnManager;
    public float finalCheckDelay = 5.0f;

    GMScript gm;
    playerManager pm;
    ingameUIScript ui;

    GameObject cue;
    
    bool isPlayer1Turn;
    bool hasAllBallsStoppedMovement = true;
    bool aBallIsMoving = false;
    bool stripeBallPotted = false;
    bool spotBallPotted = true;

    List<GameObject> ballList = new List<GameObject>();

    private void Awake()
    {
        if(turnManager == null)
        {
            turnManager = this;
        } else
        {
            Destroy(this);
        }
        gm = GMScript.gameMan;
        ui = gm.GetUIObject();
        pm = playerManager.playerMan;
        ballList = gm.GetBallList();
        cue = gm.GetCueObject();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(hasAllBallsStoppedMovement == false)
        { 
            if (aBallIsMoving == false)
            {
                Invoke("CheckFinalBallMovement", finalCheckDelay);
            } else
            {
                CheckBallMovement();
            }
        } else
        {
            GMScript.Target currentPlayerTarget;
            if(isPlayer1Turn)
            {
                currentPlayerTarget = pm.GetPlayer1Target();
            } else
            {
                currentPlayerTarget = pm.GetPlayer2Target();
            }

            if(currentPlayerTarget == GMScript.Target.None)
            {
                ui.UpdateTurnText();
            }
            isPlayer1Turn = !isPlayer1Turn;
            
        }
	}

    public void StartTurn()
    {
        stripeBallPotted = false;
        spotBallPotted = false;
    }

    public void CueBallHit()
    {
        hasAllBallsStoppedMovement = false;
        aBallIsMoving = false;
    }

    public void CheckFinalBallMovement()
    {
        CheckBallMovement();
        if(aBallIsMoving == false)
        {
            hasAllBallsStoppedMovement = true;
        }
    }

    public void CheckBallMovement()
    {
        
        foreach (GameObject obj in ballList)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (Mathf.Abs(rb.velocity.x) > 0.01f && Mathf.Abs(rb.velocity.y) > 0.2f && Mathf.Abs(rb.velocity.z) > 0.01f)
            {
                aBallIsMoving = true;
            }
        }
    }
}
