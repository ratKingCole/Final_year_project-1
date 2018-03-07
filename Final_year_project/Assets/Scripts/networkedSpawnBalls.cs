using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class networkedSpawnBalls : NetworkBehaviour
{

    [SerializeField]
    List<Object> stripesToSpawn = new List<Object>();
    [SerializeField]
    List<Object> spotsToSpawn = new List<Object>();

    public int balls = 15;
    public int rows = 5;
    public float ballRadius = 0.25f;
    public Object blackBallPrefab;
    public Object cueBallPrefab;
    public Object cuePrefab;

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

    List<GameObject> ballList = new List<GameObject>();

    public override void OnStartServer()
    {
        startSpot = true;
        gm = GMScript.gameMan;

        if (stripesToSpawn != null && spotsToSpawn != null && blackBallPrefab != null && cueBallPrefab != null)
        {
            Vector3 spawnPos = transform.position;
            float rowStartX = transform.position.x - ballRadius;
            float rowStartZ = transform.position.z - ballRadius;

            spawnPos.z = rowStartZ - (((ballRadius * 2) * 20));
            spawnPos.y += 0.01f;
            GameObject cueBallInstantiate = (GameObject)Instantiate(cueBallPrefab, spawnPos, Quaternion.identity);
            NetworkServer.Spawn(cueBallInstantiate);
            Rigidbody cueBallRb = cueBallInstantiate.GetComponent<Rigidbody>();
            gm.SetCueBall(cueBallInstantiate);
            gm.SetCueBallSpawn(spawnPos);
            cueBallRb.velocity = Vector3.zero;
            cueBallRb.angularVelocity = Vector3.zero;
            cueBallRb.rotation = Quaternion.Euler(Vector3.zero);
            ballList.Add(cueBallInstantiate);

            spawnPos.y = transform.position.y;
            for (int i = 0; i < (rows + 1); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    spawnPos.x = rowStartX - (((ballRadius * 2) * j));
                    spawnPos.z = rowStartZ;
                    if (i == 3 && j == 1)
                    {
                        GameObject obj = (GameObject)Instantiate(blackBallPrefab, spawnPos, Quaternion.identity);
                        NetworkServer.Spawn(obj);
                        ballList.Add(obj);
                    }
                    else
                    {
                        if (spawnArray[i - 1][j] == 1)
                        {
                            if (spotsToSpawn.Count > 0)
                            {
                                Object toSpawn = spotsToSpawn[0];
                                GameObject obj = (GameObject)Instantiate(toSpawn, spawnPos, Quaternion.identity);
                                NetworkServer.Spawn(obj);
                                spotsToSpawn.Remove(toSpawn);
                                ballList.Add(obj);
                            }
                        }
                        else
                        {
                            if (stripesToSpawn.Count > 0)
                            {
                                Object toSpawn = stripesToSpawn[0];
                                GameObject obj = (GameObject)Instantiate(toSpawn, spawnPos, Quaternion.identity);
                                NetworkServer.Spawn(obj);
                                stripesToSpawn.Remove(toSpawn);
                                ballList.Add(obj);
                            }
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
