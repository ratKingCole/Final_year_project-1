using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInputController : NetworkBehaviour
{

    [SerializeField]
    float CueRotationSpeed = 150f;

    private Vector3 cueRotOffset = new Vector3(0f, 0f, 0f);
    private Vector3 cuePosOffset = new Vector3(0f, 0.5f, -2.5f);

    [SerializeField]
    NetworkScript nm;

    [SerializeField]
    turnManagerScript tm;


    public static PlayerInputController Local { get; private set; }

    // Use this for initialization
    void Start()
    {
        try
        {
            nm = NetworkScript.NetScript;
            tm = turnManagerScript.turnManager;
        }
        catch { }
    }

    public void SyncIsPlayer1Turn(bool _value)
    {
        PlayerInputController.Local.Cmd_AssignLocalAuthority(turnManagerScript.turnManager.gameObject);
        PlayerInputController.Local.Cmd_SyncIsPlayer1Turn(_value, netId);
        PlayerInputController.Local.Cmd_RemoveLocalAuthority(turnManagerScript.turnManager.gameObject);
    }

    public void SyncRemainingBalls(int spotsRemaining, int stripesRemaining)
    {
        PlayerInputController.Local.Cmd_AssignLocalAuthority(turnManagerScript.turnManager.gameObject);
        PlayerInputController.Local.Cmd_SyncRemainingBalls(spotsRemaining, stripesRemaining, netId);
        PlayerInputController.Local.Cmd_RemoveLocalAuthority(turnManagerScript.turnManager.gameObject);
    }

    public void SyncBallPositions()
    {

        GameObject[] spottedBalls = GameObject.FindGameObjectsWithTag("spotBall");
        GameObject[] stripeBall = GameObject.FindGameObjectsWithTag("stripeBall");
        GameObject blackBall = GameObject.FindGameObjectWithTag("blackBall");
        GameObject cueBall = GameObject.FindGameObjectWithTag("cueBall");

        List<GameObject> balls = new List<GameObject>();
        foreach (GameObject ball in spottedBalls)
        {
            balls.Add(ball);
        }

        foreach (GameObject ball in stripeBall)
        {
            balls.Add(ball);
        }

        balls.Add(blackBall);
        balls.Add(cueBall);

        foreach (GameObject ball in balls)
        {
            ball ballScript = ball.GetComponent<ball>();
            PlayerInputController.Local.Cmd_AssignLocalAuthority(ball);
            PlayerInputController.Local.Cmd_UpdateBallPosition(ball, ball.transform.position, netId);
            PlayerInputController.Local.Cmd_RemoveLocalAuthority(ball);
        }
    }

    public void SyncPlayerTargets()
    {

        int player1TargetInt, player2TargetInt;
        GMScript.Target player1Target = playerManager.playerMan.GetPlayer1Target();
        GMScript.Target player2Target = playerManager.playerMan.GetPlayer2Target();

        if(player1Target == GMScript.Target.Spots)
        {
            player1TargetInt = 0;
        }else if(player1Target == GMScript.Target.Stripes)
        {
            player1TargetInt = 1;
        }else
        {
            player1TargetInt = 2;
        }

        if (player2Target == GMScript.Target.Spots)
        {
            player2TargetInt = 0;
        }
        else if (player2Target == GMScript.Target.Stripes)
        {
            player2TargetInt = 1;
        }
        else
        {
            player2TargetInt = 2;
        }

        PlayerInputController.Local.Cmd_AssignLocalAuthority(playerManager.playerMan.gameObject);
        PlayerInputController.Local.Cmd_SyncPlayerTargets(player1TargetInt, player2TargetInt, netId);
        PlayerInputController.Local.Cmd_RemoveLocalAuthority(playerManager.playerMan.gameObject);
    }

    public void SyncNumberOfVisits(int _visits)
    {
        PlayerInputController.Local.Cmd_AssignLocalAuthority(playerManager.playerMan.gameObject);
        PlayerInputController.Local.Cmd_SyncNumberOfVisits(_visits, netId);
        PlayerInputController.Local.Cmd_RemoveLocalAuthority(playerManager.playerMan.gameObject);
    }

    [Command]
    private void Cmd_UpdateBallPosition(GameObject ball, Vector3 position, NetworkInstanceId _netId)
    {
        NetworkServer.FindLocalObject(_netId).GetComponent<PlayerInputController>().Rpc_UpdateBallPosition(ball, position);
    }

    [ClientRpc]
    private void Rpc_UpdateBallPosition(GameObject ball, Vector3 position)
    {
        ball.transform.position = position;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if(ball.CompareTag("cueBall"))
        {
            GameObject cue = GameObject.FindGameObjectWithTag("poolCue");
            cue.GetComponent<poolCue>().ResetCue();
        }
    }

    [Command]
    public void Cmd_AssignLocalAuthority(GameObject obj)
    {
        NetworkInstanceId nIns = obj.GetComponent<NetworkIdentity>().netId;
        GameObject client = NetworkServer.FindLocalObject(nIns);
        NetworkIdentity ni = client.GetComponent<NetworkIdentity>();
        ni.AssignClientAuthority(connectionToClient);
    }

    [Command]
    public void Cmd_RemoveLocalAuthority(GameObject obj)
    {
        NetworkInstanceId nIns = obj.GetComponent<NetworkIdentity>().netId;
        GameObject client = NetworkServer.FindLocalObject(nIns);
        NetworkIdentity ni = client.GetComponent<NetworkIdentity>();
        ni.RemoveClientAuthority(ni.clientAuthorityOwner);
    }

    [Command]
    public void Cmd_SyncIsPlayer1Turn(bool _value, NetworkInstanceId _netId)
    {
        NetworkServer.FindLocalObject(_netId).GetComponent<PlayerInputController>().Rpc_SyncIsPlayer1Turn(_value);
    }

    [ClientRpc]
    public void Rpc_SyncIsPlayer1Turn(bool _value)
    {
        turnManagerScript.turnManager.isPlayer1Turn = _value;
        Debug.Log("Setting isPlayer1Turn: " + _value);
    }

    [Command]
    public void Cmd_SyncRemainingBalls(int spotsRemaining, int stripesRemaining, NetworkInstanceId _netId)
    {
        NetworkServer.FindLocalObject(_netId).GetComponent<PlayerInputController>().Rpc_SyncRemainingBalls(spotsRemaining, stripesRemaining);
    }

    [ClientRpc]
    public void Rpc_SyncRemainingBalls(int spotsRemaining, int stripesRemaining)
    {
        turnManagerScript.turnManager.numOfSpots = spotsRemaining;
        turnManagerScript.turnManager.numOfStripes = stripesRemaining;
    }

    [Command]
    public void Cmd_SyncPlayerTargets(int player1Target, int player2Target, NetworkInstanceId _netId)
    {
        NetworkServer.FindLocalObject(_netId).GetComponent<PlayerInputController>().Rpc_SyncPlayerTargets(player1Target, player2Target);
    }

    [ClientRpc]
    public void Rpc_SyncPlayerTargets(int player1Target, int player2Target)
    {
        switch (player1Target)
        {
            case 0:
                playerManager.playerMan.SetPlayer1Target(GMScript.Target.Spots);
                break;
            case 1:
                playerManager.playerMan.SetPlayer1Target(GMScript.Target.Stripes);
                break;
            default:
                playerManager.playerMan.SetPlayer1Target(GMScript.Target.None);
                break;
        }

        switch(player2Target)
        {
            case 0:
                playerManager.playerMan.SetPlayer2Target(GMScript.Target.Spots);
                break;
            case 1:
                playerManager.playerMan.SetPlayer2Target(GMScript.Target.Stripes);
                break;
            default:
                playerManager.playerMan.SetPlayer2Target(GMScript.Target.None);
                break;
        }
    }

    [Command]
    public void Cmd_SyncNumberOfVisits(int numOfVisits, NetworkInstanceId _netId)
    {
        NetworkServer.FindLocalObject(_netId).GetComponent<PlayerInputController>().Rpc_SyncNumberOfVisits(numOfVisits);
    }

    [ClientRpc]
    public void Rpc_SyncNumberOfVisits(int numOfVisits)
    {
        turnManagerScript.turnManager.currentVisits = numOfVisits;
    }


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if(Local == null)
        {
            Local = this;
        }
    }

    public void OnDestroy()
    {
        if(Local == this)
        {
            Local = null;
        }
    }


}
