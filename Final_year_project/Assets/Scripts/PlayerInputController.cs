using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInputController : NetworkBehaviour
{

    [SerializeField]
    float CueRotationSpeed = 150f;

    [SerializeField]
    NetworkScript nm;

    [SerializeField]
    turnManagerScript tm;

    [SyncVar (hook ="OnKeycodeDChange")]
    bool keycodeDPressed = false;

    [SyncVar (hook = "OnKeycodeAChange")]
    bool keycodeAPressed = false;

    [SyncVar(hook = "OnKeycodeFReleased")]
    bool keycodeFReleased = false;

    // Use this for initialization
    void Start()
    {
        nm = NetworkScript.NetScript;
        tm = turnManagerScript.turnManager;
    }

    // Update is called once per frame
    void Update()
    {
        /*f(!isLocalPlayer)
        {
            return;
        }*/

        GMScript gm = GMScript.gameMan;


        Debug.Log("Is Player 1? " + gm.GetIsPlayer1());
        Debug.Log("Is Player 1 turn? " + gm.GetIsPlayer1Turn());
        if ((gm.GetIsPlayer1() && tm.GetIsPlayer1Turn()) || (!gm.GetIsPlayer1() && !tm.GetIsPlayer1Turn()))
        {
            GetCueInput();
            GetFireInput();
        }       

    }

    void GetCueInput()
    {

        GameObject pivot = GameObject.Find("cuePivot");
        GMScript gm = GMScript.gameMan;
        GameObject cue = gm.GetCueObject();
        Debug.Log("Checking for input");

        if (Input.GetKeyDown(KeyCode.A))
        {
            Cmd_SetKeycodeAPressed(true);
            Debug.Log("A key press");
        }

        if (Input.GetKey(KeyCode.D))
        {
            Cmd_SetKeycodeDPressed(true);
            Debug.Log("D key press");
        }

        if(Input.GetKeyUp(KeyCode.D))
        {
            Cmd_SetKeycodeDPressed(false);
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            Cmd_SetKeycodeAPressed(false);
        }

    }

    void GetFireInput()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            Cmd_SetKeycodeFReleased(true);
        }
    }

    void OnKeycodeAChange(bool value)
    {
        keycodeAPressed = value;
        if (keycodeAPressed)
        {
            GMScript gm = GMScript.gameMan;
            GameObject pivot = GameObject.Find("cuePivot");
            GameObject cue = gm.GetCueObject();
            cue.transform.RotateAround(pivot.transform.position, Vector3.up, CueRotationSpeed * Time.deltaTime);
        }
    }

    void OnKeycodeDChange(bool value)
    {
        keycodeDPressed = value;
        if (keycodeDPressed)
        {
            GMScript gm = GMScript.gameMan;
            GameObject pivot = GameObject.Find("cuePivot");
            GameObject cue = gm.GetCueObject();
            cue.transform.RotateAround(pivot.transform.position, -Vector3.up, CueRotationSpeed * Time.deltaTime);
        }
    }

    void OnKeycodeFReleased(bool value)
    {
        keycodeFReleased = value;
        if (keycodeFReleased == true)
        {
            GameObject poolCue = GMScript.gameMan.GetCueObject();
            poolCue.GetComponent<poolCue>().Fire(powerSlider._powerSlider.GetSliderValue());
        }
    }

    [Command]
    void Cmd_SetKeycodeAPressed(bool _keycodeAPressed)
    {
        keycodeAPressed = _keycodeAPressed;
    }

    [Command]
    void Cmd_SetKeycodeDPressed(bool _keycodeDPressed)
    {
        keycodeDPressed = _keycodeDPressed;
    }

    [Command]
    void Cmd_SetKeycodeFReleased(bool _keycodeRReleased)
    {
        keycodeFReleased = _keycodeRReleased;
    }

    

    private void OnGUI()
    {

        GUILayout.Label("Is accepting input?" + ((GMScript.gameMan.GetIsPlayer1() && tm.GetIsPlayer1Turn()) || (!GMScript.gameMan.GetIsPlayer1() && !tm.GetIsPlayer1Turn())));
    }
}
