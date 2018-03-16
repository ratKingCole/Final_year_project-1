using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cushionHitDetection : MonoBehaviour {

    public static turnManagerScript tm;

	// Use this for initialization
	void Start () {
        tm = turnManagerScript.turnManager;
	}

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = null;
        if (collision != null)
        {
            obj = collision.gameObject;
        }

        if (obj != null)
        {
            tm.AddBallToCushionHitList(obj);
        }
    }
}
