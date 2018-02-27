using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolCue : MonoBehaviour {

    playerManager playerMan;
    GMScript gm;
    turnManagerScript tm;
    GameObject cueBall;
    GameObject pivot;
    GameObject cue;
    public Rigidbody rb;
    private Vector3 cueOffset;
    private Vector3 cRotate = new Vector3(0f, 15f, 0f);
    private Vector3 cueRotOffset = new Vector3(0f, 90f, 0f);
    private Vector3 cuePosOffset = new Vector3(0f, 0.5f, -5f);
    private Vector3 ballRotation;
    float speed = 150f;
    bool reset = true;
    bool canHit = true;
    bool fireBall = false;
    Quaternion quaternion;
    float multiplier = 2000;

    float xSpin1;
    float zSpin1;
    float pwr;

    private int count = 0;

    // Use this for initialization
    void Start () {
        playerMan = playerManager.playerMan;
        gm = GMScript.gameMan;
        tm = turnManagerScript.turnManager;
        gm.SetCueObject(this.gameObject);

        cueBall = gm.GetCueBall();
        pivot = GameObject.Find("cuePivot");
        cue = this.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        cueBall = gm.GetCueBall();
        pivot = GameObject.Find("cuePivot");
        cue = this.gameObject;

        if (cueBall != null && pivot != null)
        {
            rb = cueBall.GetComponent<Rigidbody>();

            pivot.transform.position = new Vector3(cueBall.transform.position.x, cueBall.transform.position.y, cueBall.transform.position.z);
            transform.LookAt(cueBall.transform.position + cueRotOffset);

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

    private void FixedUpdate()
    {
        if(fireBall == true)
        {
            Fire(pwr, xSpin1, zSpin1);
            fireBall = false;
        }
    }

    public void CallFire(float power, float xSpin, float zSpin)
    {
        pwr = power;
        xSpin1 = xSpin;
        zSpin1 = zSpin;

        fireBall = true;
    }

    public void Fire(float power, float xSpin, float zSpin)
    {
        if (canHit == true)
        {
            TurnOffCue();
            BallAim(power, xSpin, zSpin);

        }
    }

    private void BallAim(float power, float xSpin, float zSpin)
    {
        cueBall.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        StartCoroutine(Hit(power, xSpin, zSpin));

    }

    void timer_Tick(object sender, EventArgs e)
    {
        count += 1;
        Debug.Log(count);
    }

    private IEnumerator Hit(float power, float xSpin, float zSpin)
    {
        yield return new WaitForSeconds(0.01f);

        rb.AddRelativeForce(new Vector3(0f, 0f, power), ForceMode.Impulse);

        count = 0;
        while(count<10)
        {
            rb.AddRelativeTorque(new Vector3(xSpin, 0f, zSpin));
        }
        
        //rb.AddRelativeTorque(new Vector3(0f, 0f, 0f));

        //cueBall.GetComponent<ConstantForce>().torque = new Vector3((xSpin * multiplier), 0, (zSpin * multiplier));

        //Debug.Log(Mathf.Ceil(xSpin * multiplier));

        canHit = false;
        playerMan.SetPlayerHit(true);
        tm.CueBallHit();
    }

    void TurnOffCue()
    {
        cue.GetComponent<MeshRenderer>().enabled = false;
    }

    public void ResetCue()
    {
        if (cueBall != null && pivot != null)
        {
            pivot.transform.position = new Vector3(cueBall.transform.position.x, cueBall.transform.position.y, cueBall.transform.position.z);
            transform.LookAt(cueBall.transform.position + cueRotOffset);
        }

        cue.GetComponent<MeshRenderer>().enabled = true;
        transform.position = pivot.transform.position + cuePosOffset;
        canHit = true;
    }
}