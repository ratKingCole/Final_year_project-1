using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInputController : NetworkBehaviour {

    [SerializeField]
    float CueRotationSpeed = 150f;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        GMScript gm = GMScript.gameMan;
        if (!isLocalPlayer)
        {
            if ((gm.GetIsPlayer1() && gm.GetIsPlayer1Turn()) || !gm.GetIsPlayer1() && !gm.GetIsPlayer1Turn())
            {
                GetCueInput();
                GetFireInput();
            }
        }
    }

    void GetCueInput()
    {
       
        GameObject pivot = GameObject.Find("cuePivot");
        GMScript gm = GMScript.gameMan;
        GameObject cue = gm.GetCueObject();
        Debug.Log("Checking for input");

        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(pivot.transform.position, Vector3.up, CueRotationSpeed * Time.deltaTime);

            Debug.Log("A key press");
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(pivot.transform.position, -Vector3.up, CueRotationSpeed * Time.deltaTime);
            Debug.Log("D key press");
        }
    
    }

    void GetFireInput()
    {

    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }
}
