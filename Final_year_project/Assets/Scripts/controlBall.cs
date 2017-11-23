using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlBall : MonoBehaviour {

    public Rigidbody rb;
    public float movespeed = 5.0f;

    float horizMove;
    float vertMove;
    GMScript gm;

    bool isGameOver = false;

	// Use this for initialization
	void Start () {
        gm = GMScript.gameMan;
        gm.SetCueBall(gameObject);
        rb = GetComponent<Rigidbody>();
        horizMove = 0.0f;
        vertMove = 0.0f;
        GMScript.endGameEvent += SetEndGame;
	}


	
	// Update is called once per frame
	void Update () {
       // horizMove = Input.GetAxis("Horizontal");
        //vertMove = Input.GetAxis("Vertical");
	}
>>>>>>> cc4dcfc9098e3490062b92f003821cd198707442

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            horizMove = Input.GetAxis("Horizontal");
            vertMove = Input.GetAxis("Vertical");

        }
    }
    private void FixedUpdate()
    {
        //Vector3 forceToApply = new Vector3(horizMove * movespeed, 0.0f, vertMove * movespeed);
        //rb.AddForce(forceToApply);
    }
}
