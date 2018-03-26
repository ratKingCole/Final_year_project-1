using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBalls : MonoBehaviour {

    [SerializeField]
    List<Object> stripesToSpawn = new List<Object>();
    [SerializeField]
    List<Object> spotsToSpawn = new List<Object>();

    public int balls = 15;
    public int rows = 5;
    public float ballRadius = 0.49f;

    [SerializeField]
    Object blackBallPrefab;
    [SerializeField]
    Object cueBallPrefab;
    [SerializeField]
    Object cueStickPrefab;
   

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
        if (stripesToSpawn.Count > 0 && spotsToSpawn.Count > 0 && blackBallPrefab != null && cueBallPrefab != null)
        {
            Vector3 spawnPos = transform.position;
            float randNum = Random.Range(-0.05f, 0.05f);

            float rowStartX = transform.position.x - ballRadius;
            float rowStartZ = transform.position.z - ballRadius;        

            spawnPos.z = rowStartZ - (((ballRadius * 2) * 20));
            spawnPos.y += 0.01f;
            spawnPos.x += randNum;
            Object cueBallInstantiate = Instantiate(cueBallPrefab, spawnPos, Quaternion.identity);
            gm.SetCueBallSpawn(spawnPos);
            gm.SetCueBall((GameObject)cueBallInstantiate);
            ballList.Add((GameObject)cueBallInstantiate);
            spawnPos.x = rowStartX;
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
                            Object toSpawn = spotsToSpawn[0];
                            GameObject obj = (GameObject)Instantiate(toSpawn, spawnPos, Quaternion.identity);
                            obj.GetComponent<ball>().SetStartTurnPosition(spawnPos);
                            spotsToSpawn.Remove(toSpawn);
                            ballList.Add(obj);
                        }
                        else
                        {
                            Object toSpawn = stripesToSpawn[0];
                            GameObject obj = (GameObject)Instantiate(toSpawn, spawnPos, Quaternion.identity);
                            obj.GetComponent<ball>().SetStartTurnPosition(spawnPos);
                            stripesToSpawn.Remove(toSpawn);
                            ballList.Add(obj);
                        }
                    }
                }

                rowStartZ += ballRadius * 1.7325f;
                rowStartX += ballRadius;
                startSpot = !startSpot;
            }
            GameObject cueStickObject = poolCue.cueScript.gameObject;
            cueStickObject.gameObject.SetActive(true);
            Invoke("DelayedCueReset", 0.5f);
            gm.SetBallList(ballList);

        }
    }

    void DelayedCueReset()
    {
        GameObject cueStickObject = poolCue.cueScript.gameObject;
        cueStickObject.GetComponent<poolCue>().ResetCue();
    }
}
