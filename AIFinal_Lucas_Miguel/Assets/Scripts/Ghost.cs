using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    float timer;
    float pauseTime = 0.1f;
    float timeStep = 0.0f;
    float derp;

    private Animator anim;
    private SpriteRenderer sr;

    public bool hasDeathEnded;
    public bool deadPlayer;
    public bool isDead;

    int counter = 0;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        hasDeathEnded = false;
        deadPlayer = false;
        isDead = false;

        derp = Random.Range(1, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            anim.SetBool("isDead", true);

            if (hasDeathEnded)
            {
                sr.enabled = false;
                Destroy(this);
            }
        }
        else
        {
            float dt = Time.deltaTime;
            timer -= dt;
            if (timer > 0)
            {
                return;
            }

            Vector3 movement = Vector3.zero;

            timeStep++;
            if (timeStep >= 24)
            {
                derp = Random.Range(1, 15);
                timeStep = 0.0f;
            }

            counter++;
            int direction = (int)(derp * 3 * Mathf.PerlinNoise(counter * 0.5f, counter * 0.5f));

            switch (direction)
            {
                case 1: // Up Left
                    movement.x = -0.1f;
                    movement.y = 0.1f;
                    break;
                case 2: // Up
                    movement.x = 0.0f;
                    movement.y = 0.1f;
                    break;
                case 3: // Up Right
                    movement.x = 0.1f;
                    movement.y = 0.1f;
                    break;
                case 4: // Right
                    movement.x = 0.1f;
                    movement.y = 0.0f;
                    break;
                case 5: // Down Right
                    movement.x = 0.1f;
                    movement.y = -0.1f;
                    break;
                case 6: // Down
                    movement.x = 0.0f;
                    movement.y = -0.1f;
                    break;
                case 7: // Down Left
                    movement.x = -0.1f;
                    movement.y = -0.1f;
                    break;
                case 8: // Left
                    movement.x = -0.1f;
                    movement.y = 0.0f;
                    break;
            }

            transform.position += movement;

            if (movement != Vector3.zero)
            {
                timer = pauseTime;
            }
        }

    }

    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        hasDeathEnded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            deadPlayer = true;
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.GetComponent<PlayerAttack>().isDead = true;
        }
    }
}
