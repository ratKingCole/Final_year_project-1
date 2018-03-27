using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
1 11
2 23
3 36
4 50
5 64
6 79
7 94
8 108
9 123
10 138
*/
public class sim : MonoBehaviour
{
    public static sim simulator;
    int i = 0;
    int L = 0;
    int L2 = 0;
    Vector3 v;
    int offest = 10;
    //GameObject cue;
    ArrayList results;
    string[] results_arr;
    float start_spot = 0f;
    float start_stripe = 0f;
    Vector3[] pocket;
    Vector3[] pocket_new;
    bool running = false;
    int j = 0;
    int sim_num = 0;
    int total_sims = 0;
    bool sim_active = false;
    GMScript.Target Target;
    bool db = true;
    bool balls_moving;
    bool show_dupe = false;

    //GameObject tar;

    private void Awake()
    {
        Debug.Log("Scene name:  " + SceneManager.GetActiveScene().name);
        if(!SceneManager.GetActiveScene().name.Equals("SinglePlayerAI"))
        {
            Destroy(this);
        }

        if(simulator == null)
        {
            simulator = this;
        } else
        {
            Destroy(this);
        }
    }

    void Start()
    {

    }

    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.S)) AI();

        if (Input.GetKeyDown(KeyCode.K))
        {
           
            

        }

    }

    public void AI()
    {
        sim_active = true;
        run(possaball_shots());
    }

    string[] possaball_shots()
    {

        Target = playerManager.playerMan.GetPlayer2Target();
        if (Target == GMScript.Target.None) Target = Random.Range(0.0f, 1.0f) > .5f ? GMScript.Target.Spots : GMScript.Target.Stripes;
        string gole = Target == GMScript.Target.Spots ? "spotBall" : "stripeBall";
        if (db) print("target " + gole);
        GameObject cue = null;
        foreach (var ball in GameObject.FindGameObjectsWithTag("cueBall") as GameObject[]) cue = ball;

        ArrayList shots = new ArrayList();
        float max = 140;
        float d;
        RaycastHit hit;

        for (int angle = 0; angle < 360; angle++)
        {
            d = max;

            Vector3 v = new Vector3(0f, angle, 0f);
            Vector3 direction = Quaternion.Euler(v) * Vector3.forward;
            bool cast = Physics.Raycast(cue.transform.position, direction, out hit, max);
            d = d - hit.distance;

            while (cast && hit.collider.tag == "table")
            {
                Vector3 directionR;
                if(hit.transform.rotation.y == 90) directionR = Vector3.Reflect(direction, Vector3.right);
                else directionR = Vector3.Reflect(direction, Vector3.forward);
                //if (hit.collider.name == "EndCushion") directionR = Vector3.Reflect(direction, Vector3.right);
                //else directionR = Vector3.Reflect(direction, Vector3.forward);

                if (d > 0)
                {
                    cast = Physics.Raycast(cue.transform.position, directionR, out hit, max);
                    direction = directionR;
                }
                else break;
                if (cast) d = d - hit.distance;
            }
            if (cast && hit.collider.tag == gole)
            {
                if ((max - d) <= 11) shots.Add(angle + ":" + 1);
                if ((max - d) <= 23) shots.Add(angle + ":" + 2);
                if ((max - d) <= 36) shots.Add(angle + ":" + 3);
                if ((max - d) <= 50) shots.Add(angle + ":" + 4);
                if ((max - d) <= 64) shots.Add(angle + ":" + 5);
                if ((max - d) <= 79) shots.Add(angle + ":" + 6);
                if ((max - d) <= 94) shots.Add(angle + ":" + 7);
                if ((max - d) <= 108) shots.Add(angle + ":" + 8);
                if ((max - d) <= 123) shots.Add(angle + ":" + 9);
                if ((max - d) <= 138) shots.Add(angle + ":" + 10);
            }
        }
        string[] possaball_shots = (string[])shots.ToArray((typeof(string)));
        if (db) print("possaball_shots " + shots.Count);
        return possaball_shots;
    }
    //do runs in guroops of 100 - 400
    void run(string[] possaball_shots)
    {
        //possaball_shots = new string[] { "354:2" };
        if (db) print("start sim");
        results = new ArrayList();
        total_sims = possaball_shots.Length;
        pocket = new Vector3[6 * total_sims];
        pocket_new = new Vector3[6 * total_sims];
        Time.timeScale = 100;
        L = 0;
        L2 = 0;
        sim_num = 0;
        offest = 10;
        count = 0;
        balls_moving = true;
        for (int i = 0; i < total_sims; i++)
        {
            string[] s = possaball_shots[i].Split(':');
            StartCoroutine(simulate(float.Parse(s[0]), float.Parse(s[1]), sim_num, possaball_shots[i]));
            offest += 10;
            sim_num++;
        }
    }

    void choose_shot()
    {
        if (all_sims_done())
        {
            Time.timeScale = 1;
            if (db) print("choose_shot");
            foreach (string s2 in results) print(s2);
            int limit = 10;
            results_arr = (string[])results.ToArray((typeof(string)));
            string[] top = new string[limit];
            for (int i = 0; i < limit; i++) top[i] = top_shot();
            //for (int i = 0; i < limit; i++) { print(top[i]); }
            string best_shot = min(top);
            if (db) print("best shot " + best_shot);
            string[] s = best_shot.Split(':');
            make_shot(float.Parse(s[2]), float.Parse(s[3]));
        }
    }

    void make_shot(float angle, float power)
    {
        sim_active = false;
        GMScript.gameMan.GetCueBall().GetComponent<Rigidbody>().transform.eulerAngles = new Vector3(0f, angle, 0f);
        poolCue poolCuev = GameObject.Find("Cue_Stick").GetComponent<poolCue>();
        StartCoroutine(poolCuev.Hit(power,0f,0f));
    }

    string min(string[] arr)
    {
        float min = float.Parse(arr[0].Split(':')[1]);
        float num;
        string resuit = arr[i];
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == "") { break; }

            num = float.Parse(arr[i].Split(':')[1]);
            if (num < min)
            {
                min = num;
                resuit = arr[i];
            }
        }
        return resuit;
    }

    string top_shot()
    {
        float max = 0;
        float num;
        string result = "";
        int index = 0;

        for (int i = 0; i < results_arr.Length; i++)
        {
            string s = results_arr[i].Split(':')[0];
            if (s != "done")
            {
                num = float.Parse(s);
                if (num > max)
                {
                    max = num;
                    result = results_arr[i];
                    index = i;
                }
            }
            i++;
        }

        results_arr[index] = "done:" + results_arr[index];
        return result;
    }
    int count = 0;
    bool all_sims_done()
    {
        count++;
        return count == total_sims;

    }

    private IEnumerator simulate(float angle, float power, int sim_number, string shot)
    {
        start_spot = 0f;
        start_stripe = 0f;
        GameObject cue = Dupe(sim_number);
        cue.transform.eulerAngles = new Vector3(0f, angle, 0f);
        yield return new WaitForSeconds(0.01f);
        cue.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0f, 0f, power), ForceMode.Impulse);
        //balls_moving(sim_number);
        //print(balls_moving(sim_number));
        //yield return new WaitForSeconds(10f);
        //print(balls_moving(sim_number));
        //while (!check_mov(sim_number)) {

        //  yield return new WaitForSeconds(0.01f);


        // }


        yield return new WaitForSeconds(20f);


        Analise(sim_number, shot);
        Distroy(sim_number);
        choose_shot();

        //StartCoroutine(CheckObjectsHaveStopped(sim_number, shot));
    }




    //change this 
    //https://answers.unity.com/questions/209472/detecting-when-all-rigidbodies-have-stopped-moving.html
    IEnumerator CheckObjectsHaveStopped(int sim_number, string shot)
    {
        yield return new WaitForSeconds(0.1f);
        //print("checking... ");
        Rigidbody[] GOS = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
        bool allSleeping = false;

        while (!allSleeping)
        {
            //  yield return new WaitForSeconds(0.1f);
            allSleeping = true;

            foreach (Rigidbody GO in GOS)
            {
                if (!GO.IsSleeping())
                {
                    allSleeping = false;
                    yield return null;
                    break;
                }
            }

        }
        // print("All objects sleeping");
        //Do something 

        Analise(sim_number, shot);
        Distroy(sim_number);
        choose_shot();

        balls_moving = false;

    }

    void Analise(int sim_number, string shot)
    {
        int new_spot = 0, new_stripe = 0;
        float spot_score = 0, stripe_score = 0;
        float new_dis = 0, start_dis = 0;
        foreach (var ball in GameObject.FindGameObjectsWithTag("spotBall") as GameObject[])
        {
            if (ball.name == "dupe" + sim_number)
            {
                new_spot++;
                new_dis += distens_to_pocket(ball, pocket_new, sim_number);
            }
            else if (ball.name.Contains("Ball"))
            {
                start_dis += distens_to_pocket(ball, pocket, sim_number);
            }
        }

        spot_score += (start_dis - new_dis) / 10;
        spot_score += (start_spot - new_spot) * 10f;

        start_dis = 0f;
        new_dis = 0f;
        foreach (var ball in GameObject.FindGameObjectsWithTag("stripeBall") as GameObject[])
        {
            if (ball.name == "dupe" + sim_number)
            {
                new_stripe++;
                new_dis += distens_to_pocket(ball, pocket_new, sim_number);
            }
            else if (ball.name.Contains("Ball"))
            {
                start_dis += distens_to_pocket(ball, pocket, sim_number);
            }

        }


        stripe_score += (start_dis - new_dis) / 10;
        //print((start_dis - new_dis));
        stripe_score += (start_stripe - new_stripe) * 10;

        //print(stripe_score);
        bool cueball = false;
        if (GameObject.Find("cdupe" + sim_number)) cueball = true;



        foreach (var ball in GameObject.FindGameObjectsWithTag("blackBall") as GameObject[])
        {
            if (ball.name == "dupe" + sim_number)
            {

                new_dis += distens_to_pocket(ball, pocket_new, sim_number);
                //bias moving black ball incres chang of oponent pottiong 

            }
            else
            {
                // no black ball
                if (Target == GMScript.Target.Spots && start_spot == 0) spot_score += 100;

                if (Target == GMScript.Target.Stripes && start_stripe == 0) stripe_score += 100;

            }
            if (ball.name.Contains("Ball"))
            {
                start_dis += distens_to_pocket(ball, pocket, sim_number);
            }
        }



        if (stripe_score == Mathf.Infinity) stripe_score = 0;
        if (spot_score == Mathf.Infinity) spot_score = 0;
        //print(shot);
        //print(stripe_score);
        //print(spot_score);

        if (Target == GMScript.Target.Spots)
        {
            if (spot_score > 0 && cueball)
            {
                // dont know if this made it better of wourse
                // think its bettter
                if (new_stripe == start_stripe)
                    results.Add(spot_score + ":" + stripe_score + ":" + shot);
            }

        }
        else
        {
            if (spot_score > 0 && cueball)
            {
                if (new_spot == start_spot)
                    results.Add(stripe_score + ":" + spot_score + ":" + shot);
            }
        }
    }



    float distens_to_pocket(GameObject ball, Vector3[] pocket, int sim_number)
    {
        //for(int i = 0; i < pocket.Length; i++)print(pocket[i]);
        
        float shortest_dis = 0f;
        for (int j = 0; j < 6; j++)
        {
            float dis = 0;
            dis = Vector3.Distance(pocket[j + (sim_number * 6)], ball.transform.position);
            if (shortest_dis == 0f) shortest_dis = dis;
            else if (dis < shortest_dis) shortest_dis = dis;
        }
        return shortest_dis;
    }

    void Distroy(int sim_number)
    {
        copyBalls(sim_number, true);
        coppyTable(sim_number, true);
    }

    GameObject Dupe(int sim_number)
    {
        coppyTable(sim_number, false);
        GameObject cue = copyBalls(sim_number, false);
        return cue;
    }

    void coppyTable(int sim_number, bool distroy)
    {
        /*
        GameObject tab2 = GameObject.Find("Table");
        tab2.GetComponent<spawnBalls>().enabled = false;
        v = new Vector3(tab2.transform.position.x+20, tab2.transform.position.y - offest, tab2.transform.position.z);
        var dupe2 = Instantiate(tab2, v, Quaternion.identity);
        */


        if (!distroy)
        {
            var d = GameObject.FindGameObjectsWithTag("Table")[0].GetComponent<MeshCollider>();
            // v = new Vector3(d.transform.position.x, d.transform.position.y + offest, d.transform.position.z);
            v = new Vector3(d.transform.position.x, d.transform.position.y - offest, d.transform.position.z);
            var dupe = Instantiate(d, v, Quaternion.identity);
            dupe.name = "dupe" + sim_number;
            dupe.transform.Rotate(0, 90, 0);
            foreach (Transform child in dupe.transform)
            {
                if (child.GetComponent<Renderer>()) child.GetComponent<Renderer>().enabled = show_dupe;
                if (child.name.Contains("PotCollider"))
                {

                    pocket_new[L2] = child.position;
                    L2++;
                }
            }

            foreach (Transform child in d.transform)
            {
                //if (child.GetComponent<Renderer>()) child.GetComponent<Renderer>().enabled = show_dupe;
                if (child.name.Contains("PotCollider"))
                {

                    pocket[L] = child.position;
                    L++;
                }
            }
            //pocket[L] = d.transform.position;
            //L++;
        }
        else
        {
            foreach (var tab in GameObject.FindGameObjectsWithTag("Table") as GameObject[])
            {
                


                if (tab.name == "dupe" + sim_number && distroy)
                {
                    Destroy(tab);
                }
            }

        }
        
        
        
        /*
        foreach (var tab in GameObject.FindGameObjectsWithTag("table_component") as GameObject[])
        {
            if (!tab.name.Contains("dupe") && !distroy)
            {
                //pocket[L] = tab.transform.position;
                v = new Vector3(tab.transform.position.x, tab.transform.position.y - offest, tab.transform.position.z);
                //pocket_new[L] = v;
                var dupe = Instantiate(tab, v, Quaternion.identity);
                // dupe.GetComponent<Renderer>().enabled = show_dupe;
                dupe.name = "dupe" + sim_number;
                L++;
                dupe.transform.Rotate(0, 90, 0);
                dupe.transform.transform.localScale = new Vector3(5f, 5f, 5f);
            }
            else if (tab.name == "dupe" + sim_number && distroy)
            {
                Destroy(tab);
            }
        }
        */

        /*
        foreach (var tab in GameObject.FindGameObjectsWithTag("Pocket") as GameObject[])
        {
            if (!tab.name.Contains("dupe") && !distroy)
            {
                pocket[L] = tab.transform.position;
                v = new Vector3(tab.transform.position.x, tab.transform.position.y - offest, tab.transform.position.z);
                pocket_new[L] = v;
                //var dupe = Instantiate(tab, v, Quaternion.identity);
               // dupe.GetComponent<Renderer>().enabled = show_dupe;
                //dupe.name = "dupe" + sim_number;
                L++;
            }
            else if (tab.name == "dupe" + sim_number && distroy)
            {
                Destroy(tab);
            }
        }
        */

    }

    GameObject copyBalls(int sim_number, bool distroy)
    {
        GameObject cue = null;


        foreach (var ball in GameObject.FindGameObjectsWithTag("spotBall") as GameObject[])
        {
            if (!ball.name.Contains("dupe") && !distroy)
            {
                start_spot++;
                v = new Vector3(ball.transform.position.x, ball.transform.position.y - offest, ball.transform.position.z);
                var dupe = Instantiate(ball, v, Quaternion.identity);
                dupe.GetComponent<Renderer>().enabled = show_dupe;
                dupe.name = "dupe" + sim_number;
            }
            else if (ball.name == "dupe" + sim_number && distroy)
            {
                Destroy(ball);
            }
        }

        foreach (var ball in GameObject.FindGameObjectsWithTag("blackBall") as GameObject[])
        {
            if (!ball.name.Contains("dupe") && !distroy)
            {
                v = new Vector3(ball.transform.position.x, ball.transform.position.y - offest, ball.transform.position.z);
                var dupe = Instantiate(ball, v, Quaternion.identity);
                dupe.GetComponent<Renderer>().enabled = show_dupe;
                dupe.name = "dupe" + sim_number;
            }
            else if (ball.name == "dupe" + sim_number && distroy)
            {
                Destroy(ball);
            }
        }

        foreach (var ball in GameObject.FindGameObjectsWithTag("cueBall") as GameObject[])
        {
            if (!ball.name.Contains("dupe") && !distroy)
            {
                v = new Vector3(ball.transform.position.x, ball.transform.position.y - offest, ball.transform.position.z);
                var dupe = Instantiate(ball, v, Quaternion.identity);
                dupe.GetComponent<Renderer>().enabled = show_dupe;
                dupe.name = "cdupe" + sim_number;
                cue = dupe;
            }
            else if (ball.name == "cdupe" + sim_number && distroy)
            {
                Destroy(ball);
            }
        }

        foreach (var ball in GameObject.FindGameObjectsWithTag("stripeBall") as GameObject[])
        {
            if (!ball.name.Contains("dupe") && !distroy)
            {
                start_stripe++;
                v = new Vector3(ball.transform.position.x, ball.transform.position.y - offest, ball.transform.position.z);
                var dupe = Instantiate(ball, v, Quaternion.identity);
                dupe.GetComponent<Renderer>().enabled = show_dupe;
                dupe.name = "dupe" + sim_number;
            }
            else if (ball.name == "dupe" + sim_number && distroy)
            {
                Destroy(ball);
            }
        }
        return cue;
    }

    public bool get_sim_active()
    {
        return sim_active;
    }
}
