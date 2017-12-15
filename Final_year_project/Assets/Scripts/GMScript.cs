using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{
    public enum Target {None, Spots, Stripes };
    public static GMScript gameMan;

    GameObject cueBall;
    [SerializeField]
    GameObject cue;
    Vector3 cueBallSpawn;
    ingameUIScript ui;

    bool firstPot = true;
    bool isPlayer1Turn = true;
    bool playerHasPot = false;
    bool hasGameEnded = false;

    public delegate void PotBall();
    public static event PotBall potBallEvent;

    public delegate void EndGame();
    public static event EndGame endGameEvent;

    public delegate void StartTurn();
    public static event StartTurn startTurnEvent;

    public delegate void EndTurn();
    public static event EndTurn endTurnEvent;

    playerManager playerMan;
    [SerializeField]
    List<GameObject> ballList = new List<GameObject>();

    // Use this for initialization
    void Awake()
    {
        playerMan = playerManager.playerMan;

        if (gameMan == null)
        {
            gameMan = this;
        }
        else
        {
            Destroy(this);
        }

        playerMan.SetPlayer1Target(Target.None);
        playerMan.SetPlayer2Target(Target.None);
        endTurnEvent += FlipIsPlayer1TurnBool;
        startTurnEvent += StartNextTurn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCueBall(GameObject obj)
    {
        cueBall = obj;
        if(cue != null)
        {
            cue.GetComponent<poolCue>().ResetCue();
        }
    }

    public GameObject GetCueBall()
    {
        if (cueBall != null)
        {
            return cueBall;
        } else
        {
            return null;
        }

    }

    public void SetCueBallSpawn(Vector3 vec3)
    {
        cueBallSpawn = vec3;
    }

    public Vector3 GetCueBallSpawn()
    {
        return cueBallSpawn;
    }

    public void PottedSpotBall()
    {
        if (firstPot == true)
        {
            playerMan.AddPlayer1Score(1);
            playerMan.SetPlayer1Target(Target.Spots);
            playerMan.SetPlayer2Target(Target.Stripes);
            firstPot = false;
            SetPlayerHasPot(true);
        } else
        {
            if(playerMan.GetPlayer1Target() == GMScript.Target.Spots)
            {
                playerMan.AddPlayer1Score(1);
                SetPlayerHasPot(true);
            } else
            {
                endTurnEvent();
                Debug.Log("Oh no, you potted the wrong ball");
            }
        }

        potBallEvent();
    }

    public void PottedStripeBall()
    {
        if (firstPot == true)
        {
            playerMan.AddPlayer1Score(1);
            playerMan.SetPlayer2Target(GMScript.Target.Spots);
            playerMan.SetPlayer1Target(GMScript.Target.Stripes);
            firstPot = false;
            
        }
        else
        {
            if (playerMan.GetPlayer1Target() == GMScript.Target.Stripes)
            {
                playerMan.AddPlayer1Score(1);
            }
            else
            {
                Debug.Log("Oh no, you potted the wrong ball");
                endTurnEvent();
            }
        }
        potBallEvent();
    }

    public void PottedBlackBall()
    {
        endGameEvent();
    }

    public void PottedCueBall()
    {
        endTurnEvent();
    }

    public void FlipIsPlayer1TurnBool()
    {
        isPlayer1Turn = !isPlayer1Turn;
    }

    private void StartNextTurn()
    {
        
    }

    public bool GetIsPlayer1Turn()
    {
        return isPlayer1Turn;
    }

    public void SetPlayerHasPot(bool hasPlayerPot)
    {
        playerHasPot = hasPlayerPot;
    }

    public bool GetPlayerHasPot()
    {
        return playerHasPot;
    }

    public void SetPlayerHasPotTrue()
    {
        playerHasPot = true;
    }

    public void CallEndTurnEvent()
    {
        endTurnEvent();
    }

    public void SetUIObject(ingameUIScript ui)
    {
        this.ui = ui;
    }

    public ingameUIScript GetUIObject()
    {
        if (ui != null)
        {
            return ui;
        } else
        {
            return null;
        }
    }

    public void SetBallList(List<GameObject> list)
    {
        ballList = list;
    }

    public List<GameObject> GetBallList()
    {
        return ballList;
    }

    public void SetCueObject(GameObject obj)
    {
        cue = obj;
    }

    public void RemoveBallFromList(GameObject obj)
    {
        ballList.Remove(obj);
    }

    public GameObject GetCueObject()
    {
        return cue;
    }

    public void SetGameEnded(bool isGameOver)
    {
        hasGameEnded = isGameOver;
    }
}
