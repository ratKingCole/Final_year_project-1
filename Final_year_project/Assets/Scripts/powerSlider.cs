using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class powerSlider : MonoBehaviour {

    public static powerSlider _powerSlider;
    GMScript gm;
    NetworkScript nm;
    turnManagerScript tm;
    bool isNetworked;
    GameObject poolCue;
    public Text powerText;
    public Slider pwrSlider;

    private void Awake()
    {
        if (_powerSlider != null)
        {
            Destroy(this);
        }
        else
        {
            _powerSlider = this;
        }
    }

    void Start () {
        gm = GMScript.gameMan;
        nm = NetworkScript.NetScript;
        tm = turnManagerScript.turnManager;
        poolCue = gm.GetCueObject();
    }

    void Update()
    {
        powerText.text = pwrSlider.value.ToString();
    }

    public float GetSliderValue()
    {
        return pwrSlider.value;
    }
}