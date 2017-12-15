using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ingameUIScript : MonoBehaviour {

    GMScript gameManager;    
    playerManager playerMan;
    turnManagerScript tm;

    string player1Target;
    string player2Target;

    public Text targetText;
    public Text scoreText;
    public Text endGameText;
    public Text turnText;

	// Use this for initialization
	void Start () {
        gameManager = GMScript.gameMan;
        playerMan = playerManager.playerMan;
        tm = turnManagerScript.turnManager;
        gameManager.SetUIObject(this);

        GMScript.potBallEvent += GetTargets;
        GMScript.potBallEvent += UpdateScoreText;
        GMScript.potBallEvent += UpdateTargetText;
        
        GetTargets();
        UpdateTargetText();
        UpdateScoreText();
        UpdateTurnText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateTargetText()
    {
        if (targetText != null)
        {
            if (playerMan.GetPlayer1Score() >= 7)
            {
                targetText.text = "You must pot the black ball";
            }
            else
            {
                if (tm.GetIsPlayer1Turn())
                {
                    targetText.text = "You must pot " + player1Target;
                } else
                {
                    targetText.text = "You must pot " + player2Target;
                }
            }
        }
    }

    public void UpdateScoreText()
    {
        if(scoreText != null)
        {
            scoreText.text = "Player 1 - " + playerMan.GetPlayer1Score() + " : " + playerMan.GetPlayer2Score() + " - Player 2"; 
        }
    }

    void GetTargets()
    {
        GMScript.Target p1Target = playerMan.GetPlayer1Target();
        if (p1Target == GMScript.Target.None)
        {
            player1Target = "either";
            player2Target = "either";
        }
        else if (p1Target == GMScript.Target.Spots)
        {
            player1Target = "spots";
            player2Target = "stripes";
        }
        else
        {
            player1Target = "stripes";
            player2Target = "spots";
        }
    }

    public void EndGameUI(bool isWin)
    {
        if(isWin)
        {
            endGameText.text = "You win, congratulations!";
        } else
        {
            endGameText.text = "You lose! :(";
        }

        gameManager.SetGameEnded(true);
        endGameText.gameObject.SetActive(true);
    }

    public void UpdateTurnText()
    {
        if(tm.GetIsPlayer1Turn() == true)
        {
            turnText.text = "Player 1's turn";
        } else
        {
            turnText.text = "Player 2's turn";
        }
    }
}
