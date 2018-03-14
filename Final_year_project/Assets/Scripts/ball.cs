using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ball : NetworkBehaviour {
	public Rigidbody rb;
    //float i = 1;

    private NetworkIdentity theNetID;

    [SerializeField]
    Vector3 startTurnPosition;

    void start(){
        startTurnPosition = transform.position;
	}

    public void SetStartTurnPosition(Vector3 _pos)
    {
        startTurnPosition = _pos;
    }

    public Vector3 GetStartTurnPosition()
    {
        //Debug.Log("Returning : " + startTurnPosition);
        return startTurnPosition;
    }

    public override void OnStartLocalPlayer()
    {
        
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

    }

}
