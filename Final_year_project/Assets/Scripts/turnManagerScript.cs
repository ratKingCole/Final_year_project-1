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

    bool isPlayer1Turn = true;

    [SerializeField]
    bool hasAllBallsStoppedMovement = true;
    bool startOfTurn = true;
    bool stripeBallPotted = false;
    bool spotBallPotted = true;
    bool cueBallPotted = false;
    bool blackBallPotted = false;
    bool checkingTimerOn = false;
    bool cueBallHitBall = false;
    bool hasFoulOccured = false;

    [SerializeField]
    int numOfMovingBalls = 0;

    [SerializeField]
    List<GameObject> ballsPottedThisTurn = new List<GameObject>();

    GameObject InitBallHit;


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

                Debug.Log("Current target is: " + currentPlayerTarget);
                int spotsPottedThisTurn = 0;
                int stripesPottedThisTurn = 0;
                foreach (GameObject obj in ballsPottedThisTurn)
                {
                    if (obj.tag == "spotBall")
                    {
                        spotsPottedThisTurn += 1;
                        Debug.Log("A spot was potted");
                    }

                    if (obj.tag == "stripeBall")
                    {
                        stripesPottedThisTurn += 1;
                        Debug.Log("A stripe was potted");
                    }

                    if (obj.tag == "cueBall")
                    {
                        cueBallPotted = true;
                        hasFoulOccured = true;
                    }

                    if (obj.tag == "blackBall")
                    {
                        blackBallPotted = true;
                    }
                }

                if (!cueBallPotted && !blackBallPotted)
                {
                    if (currentPlayerTarget == GMScript.Target.Spots)
                    {
                        if (stripesPottedThisTurn <= 0)
                        {
                            if (isPlayer1Turn)
                            {
                                pm.AddPlayer1Score(spotsPottedThisTurn);
                            }
                            else
                            {
                                pm.AddPlayer2Score(spotsPottedThisTurn);
                            }
                        }
                        else
                        {
                            hasFoulOccured = true;
                        }
                    }

                    if (currentPlayerTarget == GMScript.Target.Stripes)
                    {
                        if (spotsPottedThisTurn <= 0)
                        {
                            if (isPlayer1Turn)
                            {
                                pm.AddPlayer1Score(stripesPottedThisTurn);
                            }
                            else
                            {
                                pm.AddPlayer2Score(stripesPottedThisTurn);
                            }
                        }
                        else
                        {
                            hasFoulOccured = true;
                        }
                    }

                    if (currentPlayerTarget == GMScript.Target.None)
                    {
                        GameObject firstBallPotted = null;
                        if (ballsPottedThisTurn.Count > 0)
                        {
                            firstBallPotted = ballsPottedThisTurn[0];
                        }

                        int scoreToAdd = 0;
                        if (firstBallPotted != null)
                        {
                            if (firstBallPotted.tag == "spotBall")
                            {
                                currentPlayerTarget = GMScript.Target.Spots;
                                scoreToAdd = spotsPottedThisTurn;
                            }

                            if (firstBallPotted.tag == "stripeBall")
                            {
                                currentPlayerTarget = GMScript.Target.Stripes;
                                scoreToAdd = stripesPottedThisTurn;
                            }
                        }

                        if(isPlayer1Turn)
                        {
                            pm.SetPlayer1Target(currentPlayerTarget);
                            pm.AddPlayer1Score(scoreToAdd);
                        } else
                        {
                            pm.SetPlayer2Target(currentPlayerTarget);
                            pm.AddPlayer2Score(scoreToAdd);
                        }
                    }
                }

                if (blackBallPotted)
                {
                    if (stripesPottedThisTurn > 0 || spotsPottedThisTurn > 0)
                    {
                        ui.EndGameUI(false);
                    }
                    else
                    {
                        ui.EndGameUI(true);
                    }
                }
                else
                {
                    if (hasFoulOccured)
                    {
                        isPlayer1Turn = !isPlayer1Turn;
                    }

                    if(cueBallPotted)
                    {
                        ResetCueBall();
                    }

                    if (cue == null)
                    {
                        cue = gm.GetCueObject();
                    }

                    cue.GetComponent<poolCue>().ResetCue();
                    StartTurn();

                    if (ui == null)
                    {
                        ui = gm.GetUIObject();
                    }

                    ui.UpdateTurnText();
                    ui.UpdateScoreText();
                    ui.UpdateTargetText();
                }
            }
        }
    }

    public void StartTurn()
    {
        startOfTurn = true;
        stripeBallPotted = false;
        spotBallPotted = true;
        cueBallPotted = false;
        blackBallPotted = false;
        checkingTimerOn = false;
        cueBallHitBall = false;
        hasFoulOccured = false;
        ballsPottedThisTurn = new List<GameObject>();
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

    public void AddBallToPottedList(GameObject obj)
    {
        ballsPottedThisTurn.Add(obj);
    }

    public void SetCueBallHasHitBall(bool hasHit)
    {
        cueBallHitBall = hasHit;
    }

    public void SetInitialBallHit(GameObject obj)
    {
        InitBallHit = obj;
    }

    public int GetCurrentPlayerScore()
    {
        if (isPlayer1Turn)
        {
            return pm.GetPlayer1Score();
        }
        else
        {
            return pm.GetPlayer2Score();
        }
    }

    public bool GetIsPlayer1Turn()
    {
        return isPlayer1Turn;
    }

    private void ResetCueBall()
    {
        GameObject cueBall = gm.GetCueBall();
        Rigidbody rb = cueBall.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
        cueBall.SetActive(true);
        cueBall.transform.position = gm.GetCueBallSpawn();
    }

    public GMScript.Target GetCurrentPlayerTarget()
    {
        if(isPlayer1Turn)
        {
            return pm.GetPlayer1Target(); 
        } else
        {
            return pm.GetPlayer2Target();
        }
    }

}
