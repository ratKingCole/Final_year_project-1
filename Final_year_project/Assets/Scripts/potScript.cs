using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potScript : MonoBehaviour {


    
    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "spotBall")
        {
            Destroy(collision.gameObject);
            Debug.Log("Potted spot ball");
        } else if(tag == "stripeBall")
        {
            Destroy(collision.gameObject);
            Debug.Log("Potted spot ball");
        } else if(tag == "cueBall")
        {
            Debug.Log("Oh no, you potted the cue ball!");
        } else if(tag == "blackBall")
        {
            Debug.Log("You potted the black");
            Destroy(collision.gameObject);
        }
    }
}
