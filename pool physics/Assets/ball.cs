using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {
	public Rigidbody rb;
	void start(){
		rb = GetComponent<Rigidbody> ();
	}

	private void hit(){
		rb.AddForce (new Vector3(1f,0f,30f), ForceMode.Impulse);
	}

	void onMouseDown(){
		Debug.Log ("mouse");
		Invoke("hit", 1f);
	}

	void Awake(){

		Invoke ("hit", 1f);
	}
}
