using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript : NetworkBehaviour {

    public static NetworkScript NetScript;
   
    GMScript gm;
    turnManagerScript tm;
    ingameUIScript ui;
    

    [SyncVar]
    int numOfConnectedClients = 0;

    void Awake()
    {
        if(NetScript != null)
        {
            Destroy(this);
        }

        NetScript = this;
    }


	// Use this for initialization
	void Start () {
        gm = GMScript.gameMan;
        gm.SetIsNetworked(true);
        ui = gm.GetUIObject();
        tm = turnManagerScript.turnManager;

    }

    [Command]
    void CmdAddClient()
    {
        numOfConnectedClients += 1;
    }

    [Command]
    public void CmdUiUpdateTurnText()
    {
        RpcUiUpdateTurnText();
    }

    [Command]
    public void CmdUiUpdateTargetText()
    {
        RpcUiUpdateTargetText();
    }

    [Command]
    public void CmdUiUpdateScoreText()
    {
        RpcUiUpdateScoreText();
    }

    [ClientRpc]
    public void RpcUiUpdateTurnText()
    {
        ui.UpdateTurnText();
    }

    [ClientRpc]
    public void RpcUiUpdateTargetText()
    {
        ui.UpdateTargetText();
    }

    [ClientRpc]
    public void RpcUiUpdateScoreText()
    {
        ui.UpdateScoreText();
    }


    public override void OnStartServer()
    {
        gm = GMScript.gameMan;
        gm.SetIsPlayer1(false);
    }

    [Command]
    public void CmdRotateCue(Vector3 _dir, float _speed)
    {
        RpcRotateCue(_dir, _speed);
        Debug.Log("Command CmdRotateCue called");
    }

    [ClientRpc]
    public void RpcRotateCue(Vector3 _dir, float _speed)
    {
        gm = GMScript.gameMan;
        GameObject pivot = GameObject.Find("cuePivot");
        GameObject cue = gm.GetCueObject();
        cue.transform.RotateAround(pivot.transform.position, _dir, _speed * Time.deltaTime);
        Debug.Log("Client recieved RpcRotateCue");
    }

    [Command]
    public void CmdResetCueBall()
    {
        RpcResetCueBall();
    }

    [Command]
    public void CmdResetCue()
    {
        RpcResetCue();
    }

    [Command]
    public void CmdResetBallPositions()
    {
        RpcResetBallPositions();
    }

    [ClientRpc]
    void RpcResetCueBall()
    {
        tm.ResetCueBall();
    }

    [ClientRpc]
    void RpcResetCue()
    {
        tm.ResetCue();
    }

    [ClientRpc]
    void RpcResetBallPositions()
    {
        tm.ResetBallPositions();
    }

    

}
