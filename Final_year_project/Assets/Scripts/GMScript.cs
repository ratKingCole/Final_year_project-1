using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{
    public enum Target {None, Spots, Stripes };
    public static GMScript gameMan;

    GameObject cueBall;
    Vector3 cueBallSpawn;

    Target player1Target;
    Target player2Target;
    int player1Score = 0;
    int player2Score = 0;
    bool firstPot = true;

    public delegate void PotBall();
    public static event PotBall potBallEvent;

    // Use this for initialization
    void Awake()
    {
        if (gameMan == null)
        {
            gameMan = this;
            player1Target = Target.None;
            player2Target = Target.None;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCueBall(GameObject obj)
    {
        cueBall = obj;
    }

    public GameObject GetCueBall()
    {
        return cueBall;
    }

    public void SetCueBallSpawn(Vector3 vec3)
    {
        cueBallSpawn = vec3;
    }

    public Vector3 GetCueBallSpawn()
    {
        return cueBallSpawn;
    }

    public Target GetPlayer1Target()
    {
        return player1Target;
    }

    public void SetPlayer1Score(int score)
    {
        player1Score += score;
    }

    public int GetPlayer1Score()
    {
        return player1Score;
    }

    public void PottedSpotBall()
    {
        if (firstPot == true)
        {
            player1Score += 1;
            player1Target = GMScript.Target.Spots;
            player2Target = GMScript.Target.Stripes;
            firstPot = false;
        } else
        {
            if(player1Target == GMScript.Target.Spots)
            {
                player1Score += 1;
            } else
            {
                Debug.Log("Oh no, you potted the wrong ball");
            }
        }

        potBallEvent();
    }

    public void PottedStripeBall()
    {
        if (firstPot == true)
        {
            player1Score += 1;
            player2Target = GMScript.Target.Spots;
            player1Target = GMScript.Target.Stripes;
            firstPot = false;
        }
        else
        {
            if (player1Target == GMScript.Target.Stripes)
            {
                player1Score += 1;
            }
            else
            {
                Debug.Log("Oh no, you potted the wrong ball");
            }
        }

        potBallEvent();
    }

}
