using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderScript : MonoBehaviour {

    GMScript gm;
    GameObject poolCue;
    public Text powerText;
    public Slider pwrSlider;

    public Text txtXValue;
    public Slider xSpinSlider;

    public Text txtZValue;
    public Slider zSpinSlider;



    void Start () {
        gm = GMScript.gameMan;
        poolCue = gm.GetCueObject();
    }

    void Update()
    {
        powerText.text = pwrSlider.value.ToString();
        txtXValue.text = xSpinSlider.value.ToString();
        txtZValue.text = zSpinSlider.value.ToString();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gm == null)
                gm = GMScript.gameMan;

            poolCue = gm.GetCueObject();
            poolCue.GetComponent<poolCue>().CallFire(pwrSlider.value, xSpinSlider.value, zSpinSlider.value);
        }
    }
}