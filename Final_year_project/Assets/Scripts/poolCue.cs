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
    Quaternion quaternion;

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
        cueBall.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        StartCoroutine(Hit(power));

    }
    
    private IEnumerator Hit(float power)
    {
        yield return new WaitForSeconds(0.01f);

        rb.AddRelativeForce(new Vector3(0f, 0f, power), ForceMode.Impulse);
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