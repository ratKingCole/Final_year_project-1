using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class poolCue : MonoBehaviour {

    public static poolCue cueScript;

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
    float CueRotationSpeed = 50f;
    bool reset = true;
    bool canHit = true;
    bool fireBall = false;
    Quaternion quaternion;
    float xSpin1;
    float zSpin1;
    float pwr;
    public bool spin = false;
    public bool fric = false;

    [SerializeField]
    float basePower = 5f;

    private void Awake()
    {
        if(cueScript == null)
        {
            cueScript = this;
        }
    }

    // Use this for initialization
    void Start () {
        playerMan = playerManager.playerMan;
        gm = GMScript.gameMan;
        tm = turnManagerScript.turnManager;
        gm.SetCueObject(this.gameObject);

        try
        {
            cueBall = GameObject.FindGameObjectWithTag("cueBall");
            pivot = GameObject.Find("cuePivot");
            cue = this.gameObject;
            cueBall.GetComponent<Rigidbody>().maxAngularVelocity = 0;
        }
        catch
        {
            Debug.Log("Can't find cueBall. Maybe it hasn't spawned yet?");
        }
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
            transform.LookAt(cueBall.transform.position);

            bool acceptingInput = true;

            if (GMScript.gameMan.GetIsNetworked())
            {
                if ((GMScript.gameMan.GetIsPlayer1() && turnManagerScript.turnManager.GetIsPlayer1Turn()) || (!GMScript.gameMan.GetIsPlayer1() && !turnManagerScript.turnManager.GetIsPlayer1Turn()))
                {
                    TurnOnCue();
                }
                else
                {
                    TurnOffCue();
                    acceptingInput = false;
                }
            }

            if (acceptingInput)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKey(KeyCode.A))
                        transform.RotateAround(pivot.transform.position, Vector3.up, (CueRotationSpeed - 40) * Time.deltaTime);

                    if (Input.GetKey(KeyCode.D))
                        transform.RotateAround(pivot.transform.position, -Vector3.up, (CueRotationSpeed - 40) * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    transform.RotateAround(pivot.transform.position, Vector3.up, CueRotationSpeed * Time.deltaTime);

                    //Debug.Log("A key press");
                }

                else if (Input.GetKey(KeyCode.D))
                 {
                    transform.RotateAround(pivot.transform.position, -Vector3.up, CueRotationSpeed * Time.deltaTime);
                    //Debug.Log("D key press");
                }

                if (Input.GetKey(KeyCode.R))
                    ResetCue();

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
            rb.AddTorque(new Vector3((xSpin1), 0f, (zSpin1)));
        }

        if (fric == true)
        {
            float friction = 0.01f;
            bool xSpinP = false;
            bool zSpinP = false;

            if (xSpin1 < 0)
            {
                xSpinP = false;
                xSpin1 += friction;
                rb.AddTorque(new Vector3(xSpin1, 0f, zSpin1));
            }
            if (xSpin1 > 0)
            {
                xSpinP = true;
                xSpin1 -= friction;
                rb.AddTorque(new Vector3(xSpin1, 0f, zSpin1));
            }

            if (zSpin1 < 0)
            {
                zSpinP = false;
                zSpin1 += friction;
                rb.AddTorque(new Vector3(xSpin1, 0f, zSpin1));
            }
            if (zSpin1 > 0)
            {
                zSpinP = true;
                zSpin1 -= friction;
                rb.AddTorque(new Vector3(xSpin1, 0f, zSpin1));
            }

            if ((xSpinP == true && xSpin1 <= 0 && zSpinP == false && zSpin1 >= 0) || (xSpinP == true && xSpin1 <= 0 && zSpinP == true && zSpin1 <= 0) || (xSpinP == false && xSpin1 >= 0 && zSpinP == true && zSpin1 <= 0) || (xSpinP == false && xSpin1 >= 0 && zSpinP == false && zSpin1 >= 0))
            {
                StopSpin();
                fric = false;
            }

        }
    }

    public void FricCollision()
    {
        xSpin1 = xSpin1 / 2;
        zSpin1 = zSpin1 / 2;
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

    public IEnumerator Hit(float power, float xSpin, float zSpin)
    {
        yield return new WaitForSeconds(0.01f);
        rb.AddRelativeForce(new Vector3(0f, 0f, power), ForceMode.Impulse);

        spin = true;
        yield return new WaitForSeconds(0.63f);
        Friction();

        canHit = false;
        playerMan.SetPlayerHit(true);
        tm.CueBallHit();
    }

    void TurnOffCue()
    {
        cue.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    void TurnOnCue()
    {
        cue.GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    public void ResetCue()
    {
        if (cueBall != null && pivot != null)
        {
            pivot.transform.position = new Vector3(cueBall.transform.position.x, cueBall.transform.position.y, cueBall.transform.position.z);
            transform.LookAt(cueBall.transform.position + cueRotOffset);
        }

        gm = GMScript.gameMan;
        cue = poolCue.cueScript.gameObject;  
        pivot = GameObject.Find("cuePivot");
        try
        {
            cue.GetComponentInChildren<MeshRenderer>().enabled = true;
            transform.position = pivot.transform.position + cuePosOffset;
        } catch
        {
            Debug.Log("Can't find cue object");
        }
        canHit = true;
    }

    public void StopSpin()
    {
        spin = false;
        fric = false;
    }

    public void Friction()
    {
        fric = true;
    }
}
