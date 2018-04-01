using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar {
    public Tilemap map;

    List<Node> visited = new List<Node>();
    List<Node> unvisited = new List<Node>();

    Dictionary<Node, Node> predecessorDict = new Dictionary<Node, Node>();
    Dictionary<Node, float> distanceDict = new Dictionary<Node, float>();
    Dictionary<Node, float> actualDistanceDict = new Dictionary<Node, float>();

    public void Init(Tilemap tileMap)
    {
        map = tileMap;
    
        List<Node> nodes = map.GetAllNodes();
        foreach (Node node in nodes)
        {
            distanceDict[node] = float.MaxValue;
            actualDistanceDict[node] = float.MaxValue;//
        }
	}
    
    public List<Node> Search(Node start, Node goal)
    {
        // 1. dist[s] = 0
        // 2. set all other distances to infinity
        List<Node> keys = new List<Node>(distanceDict.Keys);

        foreach (var key in keys)
        {

            distanceDict[key] = float.MaxValue;
            actualDistanceDict[key] = float.MaxValue;//
        }
        if (start == null)
        {
            Debug.Log("KEY NULL");
        }
        distanceDict[start] = 0f;
        actualDistanceDict[start] = 0f;//

        // 3. Initialize S(visited) and Q(unvisited)
        //    S, the set of visited nodes is initially empty
        //    Q, the queue initially conatains all nodes
        visited.Clear();        
        foreach (Node n in map.GetAllNodes())
        {
            unvisited.Add(n);
        }


        predecessorDict.Clear(); // to generate the result path
		
		while (unvisited.Count > 0)
        {
            // 4. select element of Q with the minimum distance
            Node u = GetClosestFromUnvisited();
            if (u == null)
            {
                Debug.Log("node U NULL");
            }
            // Check if the node u is the goal.            
            if (u == goal) break;
                        
            // 5. add u to list of S(visited)            
            visited.Add(u);

        	float shortest = float.MaxValue;            
            foreach(Node v in map.GetNeighbors(u))
            {
                if (visited.Contains(v))
                    continue;

                // 6. If new shortest path found then set new value of shortest path                
                if (distanceDict[v] > actualDistanceDict[u] + map.GetNeighborDistance(u, v) + map.GetEstimatedDistance(v, goal))
                {
                    actualDistanceDict[v] = actualDistanceDict[u] + map.GetNeighborDistance(u, v);
                    distanceDict[v] = actualDistanceDict[u] + map.GetNeighborDistance(u, v) + map.GetEstimatedDistance(v, goal);
                }

                // update predecessorDict to build the result path
                if (shortest >= map.GetNeighborDistance(u,v))
                {
                    shortest = map.GetNeighborDistance(u, v);
                    predecessorDict[v] = u;
                }
            }
        }

        List<Node> path = new List<Node>();

        // Generate the shortest path
        path.Add(goal);
        Node p = predecessorDict[goal];
        
        while (p != start)
        {
            path.Add(p);            
            p = predecessorDict[p];        
        }

        path.Reverse();
        return path;
    }
    
    Node GetClosestFromUnvisited()
    {
        float shortest = float.MaxValue;
        Node shortestNode = null;
        foreach (Node node in unvisited)
        {
            if (shortest > distanceDict[node])
            {   
                shortest = distanceDict[node];
                shortestNode = node;
                if(node == null)
                {
                    Debug.Log("CHANGING TO NULL");
                }

                //Debug.Log(shortest);
            }
            //DONT PUT DEBUGS HERE UNLESS YOU WANNA CRASH EVERYTHING
            //NOT EVEN IN IF STATEMENTS
        }

        //if (shortestNode == null)
        //{
        //    Debug.Log("shortest node NULL");
        //}

        unvisited.Remove(shortestNode);
        return shortestNode;
    }   
}
