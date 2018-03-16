﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerSlider : MonoBehaviour {

    GMScript gm;
    GameObject poolCue;
    public Text powerText;
    public Slider pwrSlider;


    void Start () {
        gm = GMScript.gameMan;
        poolCue = gm.GetCueObject();
    }

    void Update()
    {
        powerText.text = pwrSlider.value.ToString();

        if (Input.GetKeyDown(KeyCode.F))
        {
            poolCue.GetComponent<poolCue>().Fire(pwrSlider.value);
        }
    }
}