using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour {
	public Rigidbody rb;
    //float i = 1;

    void start(){
		
	}

	private void hit(){
        
        rb.AddForce (new Vector3(0f,0f,10f), ForceMode.Impulse);
	}

    private void Update()
    {
        //rb = GameObject.Find("cueBall(Clone)").GetComponent<Rigidbody>();
        //if (Input.GetKey(KeyCode.H)) Invoke("hit", 0f);
        

       // if (Mathf.Abs(rb.velocity.x) < 0.01f  && Mathf.Abs(rb.velocity.y) < 0.2f && Mathf.Abs(rb.velocity.z) < 0.01f)
           // if (Input.GetKey(KeyCode.H))
              //  Invoke("hit", 0f);
        

    }
}
