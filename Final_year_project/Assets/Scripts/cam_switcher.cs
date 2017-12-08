using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_switcher : MonoBehaviour {

    public Camera firstperson;
    public Camera overhead;
    public Camera thirdperson;
    public Camera cue;
    GMScript gm;

    void Start() {
        gm = GMScript.gameMan;
        firstperson.enabled = false;
        overhead.enabled = true;
        thirdperson.enabled = false;
        cue.enabled = false;
    }

    void Update(){
        if (Input.GetKey(KeyCode.Alpha1)) ShowOverheadView();
        if (Input.GetKey(KeyCode.Alpha2)) ShowThirdPersonView();
        if (Input.GetKey(KeyCode.Alpha3)) ShowFirstPersonView();
        if (Input.GetKey(KeyCode.Alpha4)) ShowcueView();
    }

    public void ShowcueView()
    {
        cue.enabled = true;
        firstperson.enabled = false;
        overhead.enabled = false;
        thirdperson.enabled = false;
    }
    public void ShowOverheadView(){
        cue.enabled = false;
        firstperson.enabled = false;
        overhead.enabled = true;
        thirdperson.enabled = false;
    }

    public void ShowFirstPersonView()
    {
        cue.enabled = false;
        firstperson.enabled = true;
        overhead.enabled = false;
        thirdperson.enabled = false;
    }

    public void ShowThirdPersonView()
    {
        cue.enabled = false;
        firstperson.enabled = false;
        overhead.enabled = false;
        thirdperson.enabled = true;
    }

}
