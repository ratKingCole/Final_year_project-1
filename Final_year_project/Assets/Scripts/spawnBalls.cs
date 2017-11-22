using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBalls : MonoBehaviour {


    public int balls = 15;
    public int rows = 5;
    public float ballRadius = 0.49f;

    public Object redBallPrefab;
    public Object yellBallPrefab;
    public Object blackBallPrefab;
    public Object cueBallPrefab;

    bool startSpot;
    GMScript gm;

	// Use this for initialization
	void Start () {
        Invoke("RackBalls", 1.0f);
	}

    void RackBalls()
    {
        startSpot = true;
        gm = GMScript.gameMan;
        if (redBallPrefab != null && yellBallPrefab != null && blackBallPrefab != null && cueBallPrefab != null)
        {
            Vector3 spawnPos = transform.position;
            float rowStartX = transform.position.x - ballRadius;
            float rowStartZ = transform.position.z - ballRadius;

            spawnPos.z = rowStartZ - (((ballRadius * 2) * 15));
            spawnPos.y += 0.01f;
            Instantiate(cueBallPrefab, spawnPos, Quaternion.identity);
            gm.SetCueBallSpawn(spawnPos);

            spawnPos.y = transform.position.y;
            for (int i = 0; i < (rows + 1); ++i)
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
                    else
                    {
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

                rowStartZ += ballRadius * 1.7325f;
                rowStartX += ballRadius;
                startSpot = !startSpot;
            }
        }
    }
}
