using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ingameUIScript : MonoBehaviour {

    GMScript gameManager;
    
    playerManager playerMan;
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
        gameManager.SetUIObject(this);

        GMScript.potBallEvent += GetTargets;
        GMScript.potBallEvent += UpdateScoreText;
        GMScript.potBallEvent += UpdateTargetText;
        

        GMScript.endGameEvent += EndGameUI;
        GetTargets();
        UpdateTargetText();
        UpdateScoreText();
        UpdateTurnText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateTargetText()
    {
        if (targetText != null)
        {
            if (playerMan.GetPlayer1Score() >= 7)
            {
                targetText.text = "You must pot the black ball";
            }
            else
            {
                targetText.text = "You must pot " + player1Target;
            }
        }
    }

    void UpdateScoreText()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + playerMan.GetPlayer1Score(); 
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

    public void EndGameUI()
    {
        if(playerMan.GetPlayer1Score() >= 7)
        {
            endGameText.text = "You win!";
        } else
        {
            endGameText.text = "You loose!";
        }

        endGameText.gameObject.SetActive(true);
    }

    public void UpdateTurnText()
    {
        if(gameManager.GetIsPlayer1Turn() == true)
        {
            turnText.text = "Player 1's turn";
        } else
        {
            turnText.text = "Player 2's turn";
        }
    }
}
