using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potScript : MonoBehaviour {

    GMScript gm;
    Rigidbody rb;
    GameObject colObject;

    private void Start()
    {
        gm = GMScript.gameMan;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        colObject = collision.gameObject;
        if (tag == "spotBall")
        {
            Destroy(collision.gameObject);
            Debug.Log("Potted spot ball");
            gm.PottedSpotBall();
        } else if(tag == "stripeBall")
        {
            Destroy(collision.gameObject);
            Debug.Log("Potted spot ball");
            gm.PottedStripeBall();
        } else if(tag == "cueBall")
        {
            Debug.Log("Oh no, you potted the cue ball!");
            rb = collision.gameObject.GetComponent<Rigidbody>();
            colObject.SetActive(false);
            Invoke("ResetCueBall", 1.0f);
        } else if(tag == "blackBall")
        {
            Debug.Log("You potted the black");
            Destroy(collision.gameObject);
        }
    }

    private void ResetCueBall()
    {
        
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
        colObject.SetActive(true);
        colObject.transform.position = gm.GetCueBallSpawn();

    }
}
