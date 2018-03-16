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

    [SerializeField]
    private Text targetText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text endGameText;
    [SerializeField]
    private Text turnText;
    [SerializeField]
    private GameObject ingameTextObject;
    [SerializeField]
    private GameObject colourSelectObject;

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
        Vector2 ballsRemaining = tm.GetBallsRemaining();
        if(scoreText != null)
        {
            scoreText.text = "Spots - " + (int)ballsRemaining.x + " : " + (int)ballsRemaining.y + " - Stripes"; 
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

    public void EnableColourSelectText()
    {
        ingameTextObject.SetActive(false);
        colourSelectObject.SetActive(true);
    }

    public void DisableColourSelectText()
    {
        ingameTextObject.SetActive(true);
        colourSelectObject.SetActive(false);
    }
}
