using UnityEngine;

public class cam_switcher : MonoBehaviour
{
    public Transform startMarker;
    public float speed;
    private float startTime;
    public bool moving = false;
    public string cameraName = "";
    public string currentCamera = "";
    public string previousCamera  = "";
    public int stage;
    public bool update = true;
    public bool onload = true;

    GameObject table;
    GameObject cueBall;
    void Start() {

        table = GameObject.FindGameObjectWithTag("Table");
        
    }
 
    void Update()
    {
        cueBall = GameObject.FindGameObjectWithTag("cueBall");

        if (onload)
        {
            cameraName = "cue";
            moving = true;
            onload = false;
            speed = 5f;
            startMarker = transform;
            startTime = Time.time;
        }
        if (!moving)
        {
            speed = 5f;
            startMarker = transform;
            startTime = Time.time;
            if (update)
            {
                previousCamera = currentCamera;
                update = false;
            }
           
            cameraName = "";
            stage = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || (moving && cameraName == "overhead"))
        {
            update = true;
            cameraName = "overhead";
            currentCamera = cameraName;
            
            moveCam(startMarker.position, new Vector3(0, 30, 0), table.transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || (moving && cameraName == "firstperson"))
        {
            update = true;
            cameraName = "firstperson";
            currentCamera = cameraName;
            moveCam(startMarker.position, GMScript.gameMan.GetCueBall().transform.position, new Vector3(0, 0, 10));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || (moving && cameraName == "thirdperson"))
        {
            update = true;
            cameraName = "thirdperson";
            currentCamera = cameraName;
            Vector3 pos = cueBall.transform.position;
            Vector3 end = new Vector3(pos.x, pos.y + 5, pos.z - 5);
            moveCam(startMarker.position, end, pos);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || (moving && cameraName == "cue"))
        {
            update = true;
            cameraName = "cue";
            currentCamera = cameraName;
            if (cueBall) { 
                Vector3 pos = cueBall.transform.position;
                Vector3 end = new Vector3(pos.x, pos.y + 5, pos.z - 8);
                moveCam(startMarker.position, end, pos);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || (moving && cameraName == "spin"))
        {
            cameraName = "spin";
            currentCamera = cameraName;
            Vector3 tag = table.transform.position;
            Vector3 end1 = new Vector3(0, 20, -20);
            if (stage == 1) {
                speed = 10;
                moveCam(startMarker.position, end1, tag);
                if (!moving) {
                    stage++;
                    moving = true;
                }
            }
            if(stage == 2)
            {
                speed = 50;
                transform.LookAt(table.transform.position);
                transform.RotateAround(table.transform.position, Vector3.up, speed * Time.deltaTime);
                moving = true;
            }
            if (transform.position.z < 0 && (transform.position.x >= 0.1 && transform.position.x <= 1))
            {
                speed = 1;
                stage++;
                moveCam(startMarker.position, end1, tag);
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.Alpha6) || (moving && cameraName == "spin2"))
        {
            speed = 20;
            cameraName = "spin2";
            currentCamera = cameraName;
            transform.LookAt(GameObject.Find("Table").transform.position);
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            moving = true;
        }
        */

        if (Input.GetKeyDown(KeyCode.Alpha6) || (moving && cameraName == "break"))
        {

            Vector3 tag = GameObject.FindGameObjectWithTag("ballSpawner").transform.position;
            Vector3 end = new Vector3(tag.x, tag.y + 20, tag.z);
            speed = .1f;
            cameraName = "break";
            currentCamera = cameraName;
            transform.LookAt(tag);
            moveCam(startMarker.position, end, tag);





        }

        if (Input.GetKeyDown(KeyCode.Alpha7) || (moving && cameraName == "pocket"))
        {

            Vector3 tag = GameObject.Find("pocket (3)").transform.position;
            Vector3 end = new Vector3(tag.x+5, tag.y + 10, tag.z-5);
            speed = .1f;
            cameraName = "pocket";
            currentCamera = cameraName;
            transform.LookAt(tag);
            moveCam(startMarker.position, end, tag);
        }
    }

    void moveCam(Vector3 start, Vector3 end, Vector3 target)
    {
        float distance = Vector3.Distance(start, end);
        if (distance == 0)
        {
            moving = false;
        }
        else
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / distance;
            transform.position = Vector3.Lerp(start, end, fracJourney);
            transform.LookAt(target);
            if (fracJourney > 1) moving = false;
            else moving = true;
        }
    }

}
