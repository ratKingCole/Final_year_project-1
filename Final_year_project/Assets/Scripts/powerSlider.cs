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
        poolCue = GameObject.FindGameObjectWithTag("poolCue");
    }

    void Update()
    {
        powerText.text = pwrSlider.value.ToString();

        if ((GMScript.gameMan.GetIsPlayer1() && turnManagerScript.turnManager.GetIsPlayer1Turn()) || (!GMScript.gameMan.GetIsPlayer1() && !turnManagerScript.turnManager.GetIsPlayer1Turn()))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                poolCue = GameObject.FindGameObjectWithTag("poolCue");
                poolCue.GetComponent<poolCue>().Fire(pwrSlider.value);
            }
        }
    }

    public float GetSliderValue()
    {
        return pwrSlider.value;
    }
}