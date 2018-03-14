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
    Quaternion quaternion;
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

            if ((GMScript.gameMan.GetIsPlayer1() && tm.GetIsPlayer1Turn()) || (!GMScript.gameMan.GetIsPlayer1() && !tm.GetIsPlayer1Turn()))
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

    public void Fire(float power)
    {
        if (canHit == true)
        {
            TurnOffCue();
            BallAim(power);
        }
    }

    private void BallAim(float power)
    {
        cueBall = GameObject.FindGameObjectWithTag("cueBall");
        Rigidbody rb = cueBall.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        cueBall.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        StartCoroutine(Hit(power));

    }
    
    private IEnumerator Hit(float powerModifier)
    {
        yield return new WaitForSeconds(0.01f);

        rb.AddRelativeForce(new Vector3(0f, 0f, basePower * powerModifier), ForceMode.Impulse);
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
        cue = gm.GetCueObject();
        pivot = GameObject.Find("cuePivot");
        cue.GetComponentInChildren<MeshRenderer>().enabled = true;
        transform.position = pivot.transform.position + cuePosOffset;
        canHit = true;
    }
}