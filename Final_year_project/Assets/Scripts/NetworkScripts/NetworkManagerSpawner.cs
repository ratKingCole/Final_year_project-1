using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerSpawner : NetworkBehaviour {

    [SerializeField]
    GameObject GMObject;
    [SerializeField]
    GameObject networkScriptObject;
    [SerializeField]
    GameObject turnManagerObejct;
    [SerializeField]
    GameObject playerManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnStartServer()
    {
        GameObject gm = Instantiate(GMObject, Vector3.zero, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(gm);

        GameObject nm = Instantiate(networkScriptObject, Vector3.zero, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(nm);

        GameObject tm = Instantiate(turnManagerObejct, Vector3.zero, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(tm);

        GameObject pm = Instantiate(playerManager, Vector3.zero, Quaternion.identity) as GameObject;
    }
}
