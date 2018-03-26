using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderScript : MonoBehaviour {

    public static sliderScript _sliderScript;

    GMScript gm;
    GameObject poolCue;
    public Text powerText;
    public Slider pwrSlider;

    public Text txtXValue;
    public Slider xSpinSlider;

    public Text txtZValue;
    public Slider zSpinSlider;

    private void Awake()
    {
        if(_sliderScript == null)
        {
            _sliderScript = this;
        } else
        {
            Destroy(this);
        }
    }

    void Start () {
        gm = GMScript.gameMan;
        poolCue = GameObject.FindGameObjectWithTag("poolCue");

        powerText = pwrSlider.GetComponentInChildren<Text>();
        txtXValue = xSpinSlider.GetComponentInChildren<Text>();
        txtZValue = zSpinSlider.GetComponentInChildren<Text>();
    }

    void Update()
    {
        powerText.text = pwrSlider.value.ToString();
        txtXValue.text = xSpinSlider.value.ToString();
        txtZValue.text = zSpinSlider.value.ToString();

        bool acceptingInput = true;

        if (GMScript.gameMan.GetIsNetworked())
        {
            if (!((GMScript.gameMan.GetIsPlayer1() && turnManagerScript.turnManager.GetIsPlayer1Turn()) || (!GMScript.gameMan.GetIsPlayer1() && !turnManagerScript.turnManager.GetIsPlayer1Turn())))
            {
                acceptingInput = false;
            }
        }

        if (acceptingInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (gm == null)
                    gm = GMScript.gameMan;

                poolCue = gm.GetCueObject();
                poolCue.GetComponent<poolCue>().CallFire(pwrSlider.value, xSpinSlider.value, zSpinSlider.value);
            }
        }
    }
}