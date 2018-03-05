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
   

    [SerializeField]
    float ballSpawnDelay = 1.0f;
    bool startSpot;
    GMScript gm;
    int[][] spawnArray = new int[][] {
                                     new int[] {1},
                                     new int[] {1, 0},
                                     new int[] {0, 1 ,1},
                                     new int[] {1, 1, 0, 0},
                                     new int[] {0, 1, 0, 0, 1}
                                     };

    List<GameObject> ballList;
	// Use this for initialization
	void Start () {
        ballList = new List<GameObject>();
        Invoke("RackBalls", ballSpawnDelay);
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
            Object cueBallInstantiate = Instantiate(cueBallPrefab, spawnPos, Quaternion.identity);
            gm.SetCueBallSpawn(spawnPos);
            gm.SetCueBall((GameObject)cueBallInstantiate);
            ballList.Add((GameObject)cueBallInstantiate);

            spawnPos.y = transform.position.y;
            for (int i = 0; i < (rows+1); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    spawnPos.x = rowStartX - (((ballRadius * 2) * j));
                    spawnPos.z = rowStartZ;
                    if (i == 3 && j == 1)
                    {
                        GameObject obj = (GameObject)Instantiate(blackBallPrefab, spawnPos, Quaternion.identity);
                        ballList.Add(obj);
                    }
                    else
                    {
                        if (spawnArray[i-1][j] == 1)
                        {
                            GameObject obj = (GameObject)Instantiate(redBallPrefab, spawnPos, Quaternion.identity);
                            ballList.Add(obj);
                        }
                        else
                        {
                            GameObject obj = (GameObject)Instantiate(yellBallPrefab, spawnPos, Quaternion.identity);
                            ballList.Add(obj);
                        }
                    }
                }

                rowStartZ += ballRadius * 1.7325f;
                rowStartX += ballRadius;
                startSpot = !startSpot;
            }

            gm.SetBallList(ballList);

        }
    }
}
