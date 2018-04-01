using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private bool dead = false;
    private bool canMove = true;
    public float moveSpeed = 50f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        float dt = Time.fixedDeltaTime;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float speed = 0;

        if (!dead)
        {
            // Make sure speed is always a positive number
            //if (h < 0)
            //{
            //    speed += -h;
            //}
            //else
            //{
            //    speed += h;
            //}
            //if (v < 0)
            //{
            //    speed += -v;
            //}
            //else
            //{
            //    speed += v;
            //}
            //if (speed > 0.3)
            //{
            //    anim.SetBool("Run", true);
            //}
            //else if (speed < 0.3)
            //{
            //    anim.SetBool("Run", false);
            //}
            if (canMove)
            {
                //transform.position += new Vector3(v * moveSpeed * dt, 0, -h * moveSpeed * dt);
                GetComponent<Rigidbody2D>().velocity = new Vector3(h * moveSpeed, v * moveSpeed, 0);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            }
        }
    }
}