using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedBallSync : NetworkBehaviour {

    public void UpdateBallPositions()
    { 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInputController>().SyncBallPositions();
    }


}
