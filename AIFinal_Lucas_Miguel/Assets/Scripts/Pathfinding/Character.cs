using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public Tilemap map;
    AStar aStarPathfinder = new AStar();
    public enum State { MOVE, IDLE };
    public GameObject targetTile;
    State state;

	// Use this for initialization
	void Start () {
        state = State.IDLE;
        aStarPathfinder.Init(map);	    	
	}
	
	// Update is called once per frame
	void Update () {
        // handle mouse input
        //if (state == State.IDLE && Input.GetMouseButtonDown(0))
        //{
        //
        //}

        state = State.MOVE;
        //Vector3 pos = Input.mousePosition;
        Vector3 pos = targetTile.transform.position;
        //pos = Camera.main.ScreenToWorldPoint(pos);
        
        //Debug.Log("update 1");

        Node targetNode = GetMap().GetNode(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
        //Debug.Log(pos.x + ", " + pos.y);

        if (targetNode.tileType == Node.TileType.PATH)
        {
            StartCoroutine(SearchPathAndMove(targetNode));
        }
        else
        {
            state = State.IDLE;
            Debug.Log("WALL - can't move to here");
        }

        //Debug.Log("update 2");

    }

    private Tilemap GetMap()
    {
        return map;
    }

    IEnumerator SearchPathAndMove(Node target)
    {
        // To do: Find out a start node. The start node is the node where the character stands
        // Node start = ?
        Node start = map.GetNode((int)transform.position.x, (int)transform.position.y);

        // To do: Get the shortest path between start and target using astar algorithm
        // List<Node> path = ?
        List<Node> path = aStarPathfinder.Search(start, target);

        //yield return null;
        // To do: move the character using the position info in the shortest path
        // you need to use "yield return new WaitForSeconds(0.5f);" to make a delay between each movement
        // Refer to https://docs.unity3d.com/Manual/Coroutines.html

        foreach(Node node in path)
        {
            transform.position = new Vector3(node.x, node.y, 0);
            yield return new WaitForSeconds(100f);
        }
        transform.position = new Vector3(-10, 6, 0);

        // set state to IDLE in order to enable next movement
        state = State.IDLE;
    }

    
}
