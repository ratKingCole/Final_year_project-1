using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cueBallScript : MonoBehaviour {

    public static cueBallScript cueBallSingleton;

    playerManager pm;
    GMScript gm;
    turnManagerScript tm;
    Vector3 spawnPosition;
    GameObject cueBall;
    GameObject pC;


    private void Awake()
    {
        if(cueBallSingleton == null)
        {
            cueBallSingleton = this;
        }
    }


    // Use this for initialization
    void Start () {
        pm = playerManager.playerMan;
        gm = GMScript.gameMan;
        tm = turnManagerScript.turnManager;

        gm.SetCueBall(this.gameObject);
        spawnPosition = gameObject.transform.position;

        cueBall = gm.GetCueBall();
        pC = gm.GetCueObject();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {

            if (tm == null)
            {
                tm = turnManagerScript.turnManager;
            }
            tm.SetFirstBallHitThisTurn(collision.gameObject);


            if (collision != null)
            {
                GameObject cue = GameObject.FindGameObjectWithTag("poolCue");
                cue.GetComponent<poolCue>().FricCollision();
                GameObject cueBall = GameObject.FindGameObjectWithTag("cueBall");
                cueBall.GetComponent<ConstantForce>().torque = Vector3.zero;
                GameObject.FindGameObjectWithTag("poolCue").GetComponent<poolCue>().spin = false;
            }
    
    }

    public void SetSpawnPosition(Vector3 _spawn)
    {
        spawnPosition = _spawn;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
