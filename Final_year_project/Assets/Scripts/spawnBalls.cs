using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBalls : MonoBehaviour {


    public int balls = 15;
    public int rows = 5;
    public float ballRadius = 0.75f;

    public Object redBallPrefab;
    public Object yellBallPrefab;
    public Object blackBallPrefab;
    public Object cueBallPrefab;

    bool startSpot;


	// Use this for initialization
	void Start () {
        startSpot = true;
        if (redBallPrefab != null && yellBallPrefab != null && blackBallPrefab != null && cueBallPrefab != null)
        {
            Vector3 spawnPos = transform.position;
            float rowStartX = transform.position.x - ballRadius;
            float rowStartZ = transform.position.z;

            spawnPos.z = rowStartZ - (((ballRadius * 2) * 15));
            spawnPos.y += 3f;
            Instantiate(cueBallPrefab, spawnPos, Quaternion.identity);
            spawnPos.y = transform.position.y;
            for (int i = 0; i < (rows+1); ++i)
            {
                bool isSpot = startSpot;
                for (int j = 0; j < i; j++)
                {
                    spawnPos.x = rowStartX - (((ballRadius * 2) * j));
                    spawnPos.z = rowStartZ;
                    if (i == 3 && j == 1)
                    {
                        Instantiate(blackBallPrefab, spawnPos, Quaternion.identity);
                    }
                    else {                    
                        if (isSpot)
                        {
                            Instantiate(redBallPrefab, spawnPos, Quaternion.identity);
                            isSpot = false;
                        }
                        else
                        {
                            Instantiate(yellBallPrefab, spawnPos, Quaternion.identity);
                            isSpot = true;                            
                        }
                    }
                }

                rowStartZ += ballRadius * 2;
                rowStartX += ballRadius;
                startSpot = !startSpot;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
