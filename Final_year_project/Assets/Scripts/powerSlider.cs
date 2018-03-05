using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class powerSlider : NetworkBehaviour {

    GMScript gm;
    NetworkScript nm;
    turnManagerScript tm;
    bool isNetworked;
    GameObject poolCue;
    public Text powerText;
    public Slider pwrSlider;


    void Start () {
        gm = GMScript.gameMan;
        nm = NetworkScript.NetScript;
        tm = turnManagerScript.turnManager;
        poolCue = gm.GetCueObject();
    }

    void Update()
    {
        if ((gm.GetIsPlayer1() && tm.GetIsPlayer1Turn()) || !gm.GetIsPlayer1() && !tm.GetIsPlayer1Turn())
        {
            powerText.text = pwrSlider.value.ToString();

            if (Input.GetKeyDown(KeyCode.F))
            {
                poolCue.GetComponent<poolCue>().Fire(pwrSlider.value);
            }
        }        
    }
}