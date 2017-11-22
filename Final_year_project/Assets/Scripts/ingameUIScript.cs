using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ingameUIScript : MonoBehaviour {

    GMScript gameManager;
    string player1Target;
    string player2Target;

    public Text targetText;
    public Text scoreText;

	// Use this for initialization
	void Start () {
        gameManager = GMScript.gameMan;

        GMScript.potBallEvent += GetTargets;
        GMScript.potBallEvent += UpdateScoreText;
        GMScript.potBallEvent += UpdateTargetText;

        GetTargets();
        UpdateTargetText();
        UpdateScoreText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateTargetText()
    {        
        if(targetText != null)
        {
            targetText.text = "You must pot " + player1Target;
        }
    }

    void UpdateScoreText()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + gameManager.GetPlayer1Score(); 
        }
    }

    void GetTargets()
    {
        GMScript.Target p1Target = gameManager.GetPlayer1Target();
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
}
