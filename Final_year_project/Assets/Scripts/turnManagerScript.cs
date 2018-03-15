/* The turn manager class contains all the logic for the Pool game, according to the rules found at http://www.epa.org.uk/wrules.php */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class turnManagerScript : NetworkBehaviour
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
    NetworkScript nm;

    [SerializeField]
    GameObject cue;


    public bool isPlayer1Turn = true;

    public bool isBreakShot = true;

    bool doesUserNeedToChoseColour = false;
    bool hasUserChosenColour = false;

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

    public int currentVisits = 1;

    [SerializeField]
    int visitsToBeAwardedNextTurn = 1;

    [SerializeField]
    int numOfColouredBalls = 14;

    [SyncVar]
    public int numOfSpots;
    [SyncVar]
    public int numOfStripes;

    [SerializeField]
    List<GameObject> ballsPottedThisTurn = new List<GameObject>();

    [SerializeField]
    List<GameObject> ballsThatHitCushionThisTurn = new List<GameObject>();

    [SerializeField]
    Dictionary<GameObject, Vector3> postionOfBallsAtStartOfTurn = new Dictionary<GameObject, Vector3>();

    GameObject InitBallHit;

    GameObject firstBallHitByCueBall = null;


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
        nm = NetworkScript.NetScript;

        if (gm != null)
        {
            ui = gm.GetUIObject();
            cue = gm.GetCueObject();
        }

        pm = playerManager.playerMan;
        numOfSpots = numOfColouredBalls / 2;
        numOfStripes = numOfSpots;
    }

    // Update is called once per frame
    void Update()
    {
        GetActiveBallList();
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
                if (isBreakShot)
                {
                    bool isValidBreak = false;

                    if (!doesUserNeedToChoseColour)
                    {

                        Vector2 spotStripePottedThisTurn = GetBallsPottedThisTurn();
                        int spotsPottedThisTurn = (int)spotStripePottedThisTurn.x;
                        int stripesPottedThisTurn = (int)spotStripePottedThisTurn.y;

                        if ((stripesPottedThisTurn > 0) || (spotsPottedThisTurn > 0))
                        {
                            isValidBreak = true;
                        }

                        int numBallsHitCushion = ballsThatHitCushionThisTurn.Count;
                        if (numBallsHitCushion > 4)
                        {
                            isValidBreak = true;
                        }

                        if (!isValidBreak)
                        {
                            Debug.Log("Invalid break: Resetting balls");
                            ResetCueBall();
                            ResetCue();
                            ResetBallPositions();
                            visitsToBeAwardedNextTurn = 2;
                            currentVisits = 0;
                        }
                        else
                        {
                            if (blackBallPotted)
                            {
                                ResetCueBall();
                                ResetCue();
                                ResetBallPositions();

                            }
                            else if (cueBallPotted)
                            {
                                visitsToBeAwardedNextTurn = 2;
                                currentVisits = 0;
                            }
                            else
                            {
                                if ((stripesPottedThisTurn > 0) && (spotsPottedThisTurn > 0))
                                {
                                    doesUserNeedToChoseColour = true;
                                    currentVisits += 1;
                                }
                                else
                                {
                                    if (spotsPottedThisTurn > 0)
                                    {
                                        if (isPlayer1Turn)
                                        {
                                            pm.SetPlayer1Target(GMScript.Target.Spots);
                                            pm.SetPlayer2Target(GMScript.Target.Stripes);
                                        }
                                        else
                                        {
                                            pm.SetPlayer2Target(GMScript.Target.Spots);
                                            pm.SetPlayer1Target(GMScript.Target.Stripes);
                                        }

                                        currentVisits += 1;
                                    }

                                    if (stripesPottedThisTurn > 0)
                                    {
                                        if (isPlayer1Turn)
                                        {
                                            pm.SetPlayer2Target(GMScript.Target.Spots);
                                            pm.SetPlayer1Target(GMScript.Target.Stripes);
                                        }
                                        else
                                        {
                                            pm.SetPlayer1Target(GMScript.Target.Spots);
                                            pm.SetPlayer2Target(GMScript.Target.Stripes);
                                        }

                                        currentVisits += 1;
                                    }
                                }

                                isBreakShot = false;
                            }
                        }

                    }
                }
                else
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

                    if (!doesUserNeedToChoseColour)
                    {
                        Vector2 spotStripePottedThisTurn = GetBallsPottedThisTurn();
                        int spotsPottedThisTurn = (int)spotStripePottedThisTurn.x;
                        int stripesPottedThisTurn = (int)spotStripePottedThisTurn.y;


                        bool doesPlayerHaveTarget = true;

                        if (currentPlayerTarget == GMScript.Target.None)
                        {
                            doesPlayerHaveTarget = false;
                        }

                        if (doesPlayerHaveTarget)
                        {
                            bool didCueBallHitTargetColourFirst = false;

                            if (currentPlayerTarget == GMScript.Target.Spots)
                            {
                                if (firstBallHitByCueBall != null)
                                {
                                    if (firstBallHitByCueBall.CompareTag("spotBall"))
                                    {
                                        didCueBallHitTargetColourFirst = true;
                                    }
                                }
                            }

                            if (currentPlayerTarget == GMScript.Target.Stripes)
                            {
                                if (firstBallHitByCueBall != null)
                                {
                                    if (firstBallHitByCueBall.CompareTag("stripeBall"))
                                    {
                                        didCueBallHitTargetColourFirst = true;
                                    }
                                }
                            }

                            Debug.Log("Current target is: " + currentPlayerTarget);

                            if (didCueBallHitTargetColourFirst)
                            {
                                bool didPlayerCommitFoul = HasFoulOccured();

                                if (!didPlayerCommitFoul)
                                {
                                    if (currentPlayerTarget == GMScript.Target.Spots)
                                    {
                                        if (ballsThatHitCushionThisTurn.Count > 0 || spotsPottedThisTurn > 0)
                                        {
                                            if (spotsPottedThisTurn > 0 && !didPlayerCommitFoul)
                                            {
                                                currentVisits += 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ballsThatHitCushionThisTurn.Count > 0 || stripesPottedThisTurn > 0)
                                        {
                                            if (stripesPottedThisTurn > 0 && !didPlayerCommitFoul)
                                            {
                                                currentVisits += 1;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cueBallPotted)
                                    {
                                        visitsToBeAwardedNextTurn = 2;
                                        currentVisits = 0;
                                    }

                                    if (blackBallPotted)
                                    {
                                        ui.EndGameUI(true);
                                    }
                                }
                            }
                            else
                            {
                                visitsToBeAwardedNextTurn = 2;
                                currentVisits = 0;
                            }
                        }
                        else
                        {
                            if ((stripesPottedThisTurn > 0) && (spotsPottedThisTurn > 0))
                            {
                                doesUserNeedToChoseColour = true;
                                currentVisits += 1;
                            }
                            else
                            {
                                if (spotsPottedThisTurn > 0)
                                {
                                    if (isPlayer1Turn)
                                    {
                                        pm.SetPlayer1Target(GMScript.Target.Spots);
                                        pm.SetPlayer2Target(GMScript.Target.Stripes);
                                    }
                                    else
                                    {
                                        pm.SetPlayer2Target(GMScript.Target.Spots);
                                        pm.SetPlayer1Target(GMScript.Target.Stripes);
                                    }

                                    isBreakShot = false;
                                    currentVisits += 1;
                                }

                                if (stripesPottedThisTurn > 0)
                                {
                                    if (isPlayer1Turn)
                                    {
                                        pm.SetPlayer2Target(GMScript.Target.Spots);
                                        pm.SetPlayer1Target(GMScript.Target.Stripes);
                                    }
                                    else
                                    {
                                        pm.SetPlayer1Target(GMScript.Target.Spots);
                                        pm.SetPlayer2Target(GMScript.Target.Stripes);
                                    }

                                    isBreakShot = false;
                                    currentVisits += 1;
                                }
                            }
                        }
                    }
                }

                if (doesUserNeedToChoseColour)
                {
                    if (hasUserChosenColour)
                    {
                        doesUserNeedToChoseColour = false;
                    }
                    else
                    {
                        if (ui == null)
                        {
                            ui = gm.GetUIObject();
                        }
                        ui.EnableColourSelectText();
                    }
                }


                if (cueBallPotted)
                {
                    ResetCueBall();
                    currentVisits = 0;
                }

                ResetCue();

                if (ui == null)
                {
                    ui = gm.GetUIObject();
                }

                ui.UpdateTurnText();
                ui.UpdateScoreText();
                ui.UpdateTargetText();

                Debug.Log("Updating UI");

                if (!doesUserNeedToChoseColour)
                {
                    StartTurn();
                }
            }
            else
            {
                GetPositionOfBallsAtStartOfTurn();
            }
        }
    }

    List<GameObject> GetActiveBallList()
    {
        GameObject[] spottedBalls = GameObject.FindGameObjectsWithTag("spotBall");
        GameObject[] stripeBall = GameObject.FindGameObjectsWithTag("stripeBall");
        GameObject blackBall = GameObject.FindGameObjectWithTag("blackBall");


        List<GameObject> balls = new List<GameObject>();
        foreach (GameObject ball in spottedBalls)
        {
            if (ball.GetComponent<Collider>().enabled)
            {
                balls.Add(ball);
            }
        }

        foreach (GameObject ball in stripeBall)
        {
            if (ball.GetComponent<Collider>().enabled)
            {
                balls.Add(ball);
            }
        }

        balls.Add(blackBall);

        return balls;
    }

    void StartTurn()
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
        ballsThatHitCushionThisTurn = new List<GameObject>();
        firstBallHitByCueBall = null;
        GetPositionOfBallsAtStartOfTurn();
        currentVisits -= 1;


        Debug.Log("Resetting turn");
        if (currentVisits <= 0)
        {
            currentVisits = visitsToBeAwardedNextTurn;
            visitsToBeAwardedNextTurn = 1;
            isPlayer1Turn = !isPlayer1Turn;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            try
            {
                player.GetComponent<PlayerInputController>().SyncIsPlayer1Turn(isPlayer1Turn);
                player.GetComponent<PlayerInputController>().SyncRemainingBalls(numOfSpots, numOfStripes);
                player.GetComponent<PlayerInputController>().SyncNumberOfVisits(currentVisits);
                player.GetComponent<PlayerInputController>().SyncPlayerTargets();
                player.GetComponent<PlayerInputController>().SyncIsBreak(isBreakShot);
                player.GetComponent<PlayerInputController>().SyncBallPositions();
            }
            catch
            {
                Debug.Log("Cannot find player. Maybe they have not spawned yet?");
            }
        }

        ResetCue();
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
        foreach (GameObject obj in GetActiveBallList())
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (Mathf.Abs(rb.velocity.x) < 0.05f && Mathf.Abs(rb.velocity.y) < 0.2f && Mathf.Abs(rb.velocity.z) < 0.05f)
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

    public void ResetCueBall()
    {
        GameObject cueBall = GMScript.gameMan.GetCueBall();
        Rigidbody rb = cueBall.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
        cueBall.SetActive(true);
        cueBall.transform.position = cueBallScript.cueBallSingleton.GetSpawnPosition();
        cueBall.transform.rotation = Quaternion.Euler(Vector3.zero);
        cueBall.GetComponent<Collider>().enabled = true;
        cueBall.GetComponent<Rigidbody>().useGravity = true;
        cueBall.GetComponent<MeshRenderer>().enabled = true;
    }

    public void ResetCue()
    {
        if (cue == null)
        {
            cue = GameObject.FindGameObjectWithTag("poolCue");
        }

        cue.GetComponent<poolCue>().ResetCue();
    }

    public void ResetBallPositions()
    {

        List<GameObject> ballList = GetActiveBallList();
        int _numOfSpots = 0;
        int _numOfStripes = 0;

        foreach (GameObject obj in ballList)
        {

            obj.transform.position = obj.GetComponent<ball>().GetStartTurnPosition();
            obj.transform.rotation = Quaternion.Euler(Vector3.zero);
            obj.GetComponent<Collider>().enabled = true;
            obj.GetComponent<Rigidbody>().useGravity = true;
            obj.GetComponentInChildren<MeshRenderer>().enabled = true;

            if (obj.CompareTag("spotBall"))
            {
                _numOfSpots += 1;
            }

            if (obj.CompareTag("stripeBall"))
            {
                _numOfStripes += 1;
            }
        }

        numOfSpots = _numOfSpots;
        numOfStripes = _numOfStripes;

    }

    private void GetPositionOfBallsAtStartOfTurn()
    {
        List<GameObject> ballList = GetActiveBallList();
        foreach (GameObject obj in ballList)
        {
            try
            {
                Vector3 _position = obj.transform.position;
                obj.GetComponent<ball>().SetStartTurnPosition(_position);
            }
            catch
            {
                Debug.Log("Problem getting ball position. Maybe ball not spawned yet?");
            }
            //postionOfBallsAtStartOfTurn.Add(obj, _position);
        }
    }

    public GMScript.Target GetCurrentPlayerTarget()
    {
        if (isPlayer1Turn)
        {
            return pm.GetPlayer1Target();
        }
        else
        {
            return pm.GetPlayer2Target();
        }
    }

    private Vector2 GetBallsPottedThisTurn()
    {
        int spotsPottedThisTurn = 0;
        int stripesPottedThisTurn = 0;
        foreach (GameObject obj in ballsPottedThisTurn)
        {
            if (obj.tag == "spotBall")
            {
                spotsPottedThisTurn += 1;
                Debug.Log("A spot was potted");
                if (numOfSpots > 0)
                {
                    numOfSpots -= 1;
                }
                else
                {
                    numOfSpots = 0;
                }
            }

            if (obj.tag == "stripeBall")
            {
                stripesPottedThisTurn += 1;
                Debug.Log("A stripe was potted");
                if (numOfStripes > 0)
                {
                    numOfStripes -= 1;
                }
                else
                {
                    numOfStripes = 0;
                }
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

        return new Vector2(spotsPottedThisTurn, stripesPottedThisTurn);
    }

    public void AddBallToCushionHitList(GameObject obj)
    {
        ballsThatHitCushionThisTurn.Add(obj);
    }

    public void PlayerHasChosenColour()
    {
        hasUserChosenColour = true;
        ui.DisableColourSelectText();
        ui.UpdateTargetText();
    }

    public void SetFirstBallHitThisTurn(GameObject _obj)
    {
        if (firstBallHitByCueBall == null)
        {
            firstBallHitByCueBall = _obj;
        }
    }

    private bool HasFoulOccured()
    {
        bool _hasFoulOccured = false;

        if (cueBallPotted)
        {
            _hasFoulOccured = true;
        }

        if (blackBallPotted)
        {
            if (GetCurrentPlayerTarget() == GMScript.Target.Spots)
            {
                if (numOfSpots > 0)
                {
                    _hasFoulOccured = true;
                }
            }
            else if (GetCurrentPlayerTarget() == GMScript.Target.Stripes)
            {
                if (numOfStripes > 0)
                {
                    _hasFoulOccured = true;
                }
            }
        }

        return _hasFoulOccured;
    }

    public Vector2 GetBallsRemaining()
    {
        return new Vector2(numOfSpots, numOfStripes);
    }

    private void OnGUI()
    {
        GUILayout.Box("Number of remaining visits: " + currentVisits);
    }
}
