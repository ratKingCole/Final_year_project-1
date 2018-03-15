using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
    private Vector3 cueRotOffset = new Vector3(0f, 0f, 0f);
    private Vector3 cuePosOffset = new Vector3(0f, 0.5f, -3.5f);
    private Vector3 ballRotation;
    float CueRotationSpeed = 150f;
    bool reset = true;
    bool canHit = true;
    bool fireBall = false;
    Quaternion quaternion;
    float xSpin1;
    float zSpin1;
    float pwr;
    float multiplier = 10;
    bool spin = false;

    [SerializeField]
    float basePower = 5f;

    // Use this for initialization
    void Start () {
        playerMan = playerManager.playerMan;
        gm = GMScript.gameMan;
        tm = turnManagerScript.turnManager;
        gm.SetCueObject(this.gameObject);

        cueBall = gm.GetCueBall();
        pivot = GameObject.Find("cuePivot");
        cue = this.gameObject;
        cueBall.GetComponent<Rigidbody>().maxAngularVelocity = 0;
    }
	
	// Update is called once per frame
	void Update () {

        cueBall = GameObject.FindGameObjectWithTag("cueBall");
        pivot = GameObject.Find("cuePivot");

        cue = this.gameObject;

        if (cueBall != null && pivot != null)
        {
            rb = cueBall.GetComponent<Rigidbody>();

            pivot.transform.position = new Vector3(cueBall.transform.position.x, cueBall.transform.position.y, cueBall.transform.position.z);
            transform.LookAt(cueBall.transform.position + cueRotOffset);

            if ((GMScript.gameMan.GetIsPlayer1() && turnManagerScript.turnManager.GetIsPlayer1Turn()) || (!GMScript.gameMan.GetIsPlayer1() && !turnManagerScript.turnManager.GetIsPlayer1Turn()))
            {
                TurnOnCue();
                if (Input.GetKey(KeyCode.A))
                {
                    transform.RotateAround(pivot.transform.position, Vector3.up, CueRotationSpeed * Time.deltaTime);

                    //Debug.Log("A key press");
                }

                if (Input.GetKey(KeyCode.D))
                {
                    transform.RotateAround(pivot.transform.position, -Vector3.up, CueRotationSpeed * Time.deltaTime);
                    //Debug.Log("D key press");
                }
            } else
            {
                TurnOffCue();
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
        
        if(spin == true)
        {
            rb.AddTorque(new Vector3(0f, 0f, 10f));
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
        cueBall = GameObject.FindGameObjectWithTag("cueBall");
        Rigidbody rb = cueBall.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        cueBall.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        StartCoroutine(Hit(power, xSpin, zSpin));

    }

    private IEnumerator Hit(float power, float xSpin, float zSpin)
    {
        yield return new WaitForSeconds(0.01f);
        rb.AddRelativeForce(new Vector3(0f, 0f, power), ForceMode.Impulse);

        spin = true;
        yield return new WaitForSeconds(2);

        //cueBall.GetComponent<ConstantForce>().torque = new Vector3((xSpin * multiplier), 0f, (zSpin * multiplier));
        //Debug.Log(Mathf.Ceil(xSpin * multiplier));

        canHit = false;
        playerMan.SetPlayerHit(true);
        tm.CueBallHit();
    }

    void TurnOffCue()
    {
        cue.GetComponent<MeshRenderer>().enabled = false;
    }

    void TurnOnCue()
    {
        cue.GetComponent<MeshRenderer>().enabled = true;
    }

    public void ResetCue()
    {
        if (cueBall != null && pivot != null)
        {
            pivot.transform.position = new Vector3(cueBall.transform.position.x, cueBall.transform.position.y, cueBall.transform.position.z);
            transform.LookAt(cueBall.transform.position + cueRotOffset);
        }

        gm = GMScript.gameMan;
        cue = gm.GetCueObject();
        pivot = GameObject.Find("cuePivot");
        cue.GetComponent<MeshRenderer>().enabled = true;
        transform.position = pivot.transform.position + cuePosOffset;
        canHit = true;
    }
}
