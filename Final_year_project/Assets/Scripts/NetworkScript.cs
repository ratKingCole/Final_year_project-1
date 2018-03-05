using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript : NetworkBehaviour {

    public static NetworkScript NetScript;
   
    GMScript gm;

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
    }

    [Command]
    public void CmdFire(float _value)
    {
        GameObject poolCue = gm.GetCueObject();
        poolCue.GetComponent<poolCue>().Fire(_value);
    }


    [Command]
    void CmdAddClient()
    {
        numOfConnectedClients += 1;
    }



}
