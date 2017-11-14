﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gameMan;

    GameObject cueBall;
    Vector3 cueBallSpawn;


    // Use this for initialization
    void Start()
    {
        if(gameMan == null)
        {
            gameMan = this;
        } else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCueBall(GameObject obj)
    {
        cueBall = obj;
    }

    public GameObject GetCueBall()
    {
        return cueBall;
    }

    public void SetCueBallSpawn(Vector3 vec3)
    {
        cueBallSpawn = vec3;
    }

    public Vector3 GetCueBallSpawn()
    {
        return cueBallSpawn;
    }

}
