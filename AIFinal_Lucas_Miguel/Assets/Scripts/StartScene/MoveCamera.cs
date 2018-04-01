using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCamera : MonoBehaviour {

    public List<GameObject> waypoints;
    int curWaypointIndex = -1;

    public Camera mainCamera;

    private Vector3 pos;
    private Vector3 target;
    private float speed;

    // Use this for initialization
    void Start () {

        target = GetNextWaypoint();

        pos = mainCamera.transform.position;
        speed = 2f;
    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        
        pos = mainCamera.transform.position;

        if ((pos.x - target.x) < 0.2f && (pos.x - target.x) > -0.2f && (pos.y - target.y) < 0.2f && (pos.y - target.y) > -0.2f)
        {
            target = GetNextWaypoint();
        }

        if (curWaypointIndex == 0)
        {
            pos = pos + (Vector3.up * speed * dt);
        }
        else if (curWaypointIndex == 1)
        {
            pos = pos + (Vector3.right * speed * dt);
        }
        else if (curWaypointIndex == 2)
        {
            pos = pos + (Vector3.down * speed * dt);
        }
        else if (curWaypointIndex == 3)
        {
            pos = pos + (Vector3.left * speed * dt);
        }
        
        mainCamera.transform.position = pos;
    }

    Vector3 GetNextWaypoint()
    {
        if (waypoints.Count < 2)
            return mainCamera.transform.position;

        curWaypointIndex++;
        if (curWaypointIndex >= waypoints.Count)
        {
            curWaypointIndex = 0;
        }

        return waypoints[curWaypointIndex].transform.position;
    }

    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
