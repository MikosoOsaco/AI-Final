using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour {
    public GameObject target;
    public float speed = 3.0f;
    public float maxAngularSpeed = 2.5f;
    Vector2 velocity = Vector2.zero;
    Vector2 orientation = Vector2.up;

    Animator anim;
    SpriteRenderer sr;

    public bool hasDeathEnded;
    public bool deadPlayer;
    public bool isDead;

    public Transform[] spawnPositions;

    float time;


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        hasDeathEnded = false;
        deadPlayer = false;
        isDead = false;

        transform.position = spawnPositions[Random.Range(0, 3)].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((target.GetComponent<Player>() != null))
        {
            if (target.GetComponent<Player>().canMove == true)
            {
                if (isDead)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    anim.SetBool("isDead", true);

                    if (hasDeathEnded)
                    {
                        sr.enabled = false;
                    }

                    time += Time.deltaTime;

                    if (time >= 1.0f)
                    {
                        transform.position = spawnPositions[Random.Range(0, 3)].transform.position;
                    }
                    if (time >= 2.0f)
                    {
                        GetComponent<BoxCollider2D>().enabled = true;
                        anim.SetBool("isDead", false);
                        sr.enabled = true;

                        hasDeathEnded = false;
                        deadPlayer = false;
                        isDead = false;
                        time = 0.0f;
                    }
                }
                else
                {
                    Vector2 targetPos = target.transform.position;
                    Vector2 pos = transform.position;
                    float dt = Time.deltaTime;

                    Vector3 dir = targetPos - pos;
                    dir.Normalize();

                    Vector2 relativeVelocity = target.GetComponent<Rigidbody2D>().velocity - velocity;
                    Vector2 relativePosition = target.GetComponent<Rigidbody2D>().position - pos;
                    float timeToClose = Vector3.Magnitude(relativePosition) / Vector3.Magnitude(relativeVelocity);
                    Vector2 predictedPosTarget = target.GetComponent<Rigidbody2D>().position + target.GetComponent<Rigidbody2D>().velocity * timeToClose;

                    dir = predictedPosTarget - pos;
                    velocity = orientation.normalized * speed;

                    transform.position += new Vector3(velocity.x, velocity.y, 0.0f) * dt;

                    float angle = Mathf.Acos(Vector3.Dot(dir, orientation) / (Vector3.Magnitude(dir) * Vector3.Magnitude(orientation)));
                    float maxAngle = maxAngularSpeed * dt;

                    if (angle > maxAngle)
                    {
                        Vector3 orientationNormal;
                        orientationNormal.x = orientation.y;
                        orientationNormal.y = -orientation.x;
                        orientationNormal.z = 0.0f;
                        orientationNormal.Normalize();

                        if (Vector3.Dot(orientationNormal, dir) > 0)
                            maxAngle *= -1;

                        Vector3 finalOrientation = Vector3.zero;
                        finalOrientation.x = orientation.x * (Mathf.Cos(maxAngle)) - orientation.y * (Mathf.Sin(maxAngle));
                        finalOrientation.y = orientation.x * (Mathf.Sin(maxAngle)) + orientation.y * (Mathf.Cos(maxAngle));

                        orientation = Vector3.Normalize(finalOrientation);
                    }

                    if (target.transform.position.x > transform.position.x)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else if (target.transform.position.x < transform.position.x)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                    }
                }
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
            time = 0.0f;
        }
    }
}
