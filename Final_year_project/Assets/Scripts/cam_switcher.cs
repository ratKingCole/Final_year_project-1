using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_switcher : MonoBehaviour {

    public Camera firstperson;
    public Camera overhead;
    public Camera thirdperson;

  void Start() {
        firstperson.enabled = false;
        overhead.enabled = true;
        thirdperson.enabled = false;
    }

    void Update(){
        if (Input.GetKey(KeyCode.Alpha1)) ShowOverheadView();
        if (Input.GetKey(KeyCode.Alpha2)) ShowThirdPersonView();
        if (Input.GetKey(KeyCode.Alpha3)) ShowFirstPersonView();
    }

    public void ShowOverheadView(){
        firstperson.enabled = false;
        overhead.enabled = true;
        thirdperson.enabled = false;
    }

    public void ShowFirstPersonView()
    {
        firstperson.enabled = true;
        overhead.enabled = false;
        thirdperson.enabled = false;
    }

    public void ShowThirdPersonView()
    {
        firstperson.enabled = false;
        overhead.enabled = false;
        thirdperson.enabled = true;
    }

}
