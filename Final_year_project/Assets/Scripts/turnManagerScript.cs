using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManagerScript : MonoBehaviour
{

    public static turnManagerScript turnManager;
    public float finalCheckDelay = 5.0f;

    [SerializeField]
    GMScript gm;
    [SerializeField]
    playerManager pm;
    [SerializeField]
    ingameUIScript ui;

    [SerializeField]
    GameObject cue;

    bool isPlayer1Turn;

    [SerializeField]
    bool hasAllBallsStoppedMovement = true;
    bool startOfTurn = true;
    bool stripeBallPotted = false;
    bool spotBallPotted = true;
    bool checkingTimerOn = false;

    [SerializeField]
    int numOfMovingBalls = 0;

    [SerializeField]
    List<GameObject> ballsPottedThisTurn = new List<GameObject>();


    private void Awake()
    {
        if (turnManager == null)
        {
            turnManager = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Use this for initialization
    void Start()
    {

        gm = GMScript.gameMan;

        if (gm != null)
        {
            ui = gm.GetUIObject();
            cue = gm.GetCueObject();
        }

        pm = playerManager.playerMan;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAllBallsStoppedMovement == false)
        {
            if (checkingTimerOn == false)
            {
                Invoke("CheckFinalBallMovement", finalCheckDelay);
                checkingTimerOn = true;
            }
            else
            {
                CheckBallMovement();
            }
        }
        else
        {
            if (startOfTurn == false)
            {
                GMScript.Target currentPlayerTarget;
                if (isPlayer1Turn)
                {
                    currentPlayerTarget = pm.GetPlayer1Target();
                }
                else
                {
                    currentPlayerTarget = pm.GetPlayer2Target();
                }

                if (currentPlayerTarget == GMScript.Target.None)
                {
                    if (ui == null)
                    {
                        ui = gm.GetUIObject();
                    }

                    ui.UpdateTurnText();
                }

                isPlayer1Turn = !isPlayer1Turn;
                if (cue == null)
                {
                    cue = gm.GetCueObject();
                }

                cue.GetComponent<poolCue>().ResetCue();
                StartTurn();
            }
        }
    }

    public void StartTurn()
    {
        stripeBallPotted = false;
        spotBallPotted = false;
        startOfTurn = true;
    }

    public void CueBallHit()
    {
        hasAllBallsStoppedMovement = false;
        startOfTurn = false;
    }

    public void CheckFinalBallMovement()
    {
        Debug.Log("Checking final ball movement");
        CheckBallMovement();
        if (numOfMovingBalls <= 0)
        {
            hasAllBallsStoppedMovement = true;
        }
        checkingTimerOn = false;
    }

    public void CheckBallMovement()
    {
        int localNumOfMovingBalls = 0;
        foreach (GameObject obj in gm.GetBallList())
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (Mathf.Abs(rb.velocity.x) < 0.01f && Mathf.Abs(rb.velocity.y) < 0.2f && Mathf.Abs(rb.velocity.z) < 0.01f)
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.angularVelocity = new Vector3(0f, 0f, 0f);
            }
            else
            {
                localNumOfMovingBalls += 1;
            }
        }

        numOfMovingBalls = localNumOfMovingBalls;
    }
}
