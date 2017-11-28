using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolCue : MonoBehaviour {

    GameObject cueBall;
    GameObject pivot;
    public Rigidbody rb;
    private Vector3 cueOffset;
    private Vector3 cRotate = new Vector3(0f, 15f, 0f);
    private Vector3 cueRotOffset = new Vector3(0f, 90f, 0f);
    private Vector3 cuePosOffset = new Vector3(0f, 0.5f, -5f);
    private Vector3 ballRotation;
    float speed = 150f;
    bool reset = true;
    bool canHit = true;
    Quaternion quaternion;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        cueBall = GameObject.Find("cueBall(Clone)");
        pivot = GameObject.Find("cuePivot");

        if (cueBall != null && pivot != null)
        {
            rb = cueBall.GetComponent<Rigidbody>();

            pivot.transform.position = new Vector3(cueBall.transform.position.x, cueBall.transform.position.y, cueBall.transform.position.z);
            //transform.SetParent(pivot.transform);
            //transform.position = new Vector3(pivot.transform.position.x + cueOffset.x, pivot.transform.position.y + cueOffset.y, pivot.transform.position.z + cueOffset.z);
            transform.LookAt(cueBall.transform.position + cueRotOffset);

            if (reset == true)
            {
                if (Mathf.Abs(rb.velocity.x) < 0.01f && Mathf.Abs(rb.velocity.y) < 0.2f && Mathf.Abs(rb.velocity.z) < 0.01f)
                {
                    transform.position = pivot.transform.position + cuePosOffset;
                    reset = false;
                    canHit = true;
                }
                else
                {
                    reset = true;
                    canHit = false;
                }
            }

            if (canHit == true)
                if (Input.GetKey(KeyCode.H))
                    Invoke("ballAim", 0f);

            if (Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(pivot.transform.position, Vector3.up, speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(pivot.transform.position, -Vector3.up, speed * Time.deltaTime);
            }
        }
    }

    private void ballAim()
    {
        //cueBall.transform.rotation = Quaternion.LookRotation(ballRotation);
        cueBall.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        Invoke("hit", 3f);
    }

    private void hit()
    {
        rb.AddRelativeForce(new Vector3(0f, 0f, 0.25f), ForceMode.Impulse);
        reset = true;
    }
}