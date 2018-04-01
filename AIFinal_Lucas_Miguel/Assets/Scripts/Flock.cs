using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
    public int maxBoids;
    public GameObject boid;
    //public GameObject[] boids;
    List<GameObject> boids = new List<GameObject>();

	// Use this for initialization
	void Start () {
		for (int i=0; i<maxBoids; i++)
        {
            boids.Add(Instantiate(boid, transform.position, transform.rotation));
        }

        foreach(GameObject boid in boids)
        {            
            boid.SetActive(true);
            boid.GetComponent<Boid>().flock = this;
        }
        
	}

	// Update is called once per frame
	void Update () {
	}

    public List<GameObject> GetBoids()
    {
        return boids;
    }
}
