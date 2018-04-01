using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fsmFighter : MonoBehaviour {

    private bool chasePlayer = false;
    private bool playerInArea = false;
    private bool attackPlayer = false;

    // waypoints to patrol
    public List<GameObject> waypoints;
    int curWaypointIndex = -1;

    public enum State { PATROL, CHASE, TRACK } // for FSM
    State state;

    // target object
    public GameObject player;
    private Vector3 playerPos;

    public GameObject childEnemy;
    private Vector3 pos;

    private Vector3 target;

    
    private Animator anim;
    private SpriteRenderer sr;

    private AudioSource audioSource;
    public AudioClip attack;
    public AudioClip chase;

    // Sentry moving speed
    public float speed;


    private Vector3 chaseDir;
    private Vector3 chaseVel;
    private Vector3 orientation;
    private float maxAngularSpeed;

    // Use this for initialization
    void Start ()
    {
        state = State.PATROL; // Initial state
        target = GetNextWaypoint().transform.position;

        pos = childEnemy.transform.position;
        playerPos = player.transform.position;
        orientation = childEnemy.transform.up;
        speed = 5f;
        maxAngularSpeed = 5f;

        anim = this.transform.GetChild(0).GetComponent<Animator>();
        sr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        anim.SetInteger("FighterAnim", 1);
    }

    GameObject GetNextWaypoint()
    {
        if (waypoints.Count < 2)
            return childEnemy;

        curWaypointIndex++;
        if (curWaypointIndex >= waypoints.Count)
        {
            curWaypointIndex = 0;
        }

        return waypoints[curWaypointIndex];
    }

    // Update is called once per frame
    void Update () {

        if (childEnemy.GetComponent<EnemyAttack>().isDead)
        {
            childEnemy.GetComponent<BoxCollider2D>().enabled = false;
            anim.SetInteger("FighterAnim", 4);

            if (childEnemy.GetComponent<EnemyAttack>().hasDeathEnded)
            {
                sr.enabled = false;
                Destroy(this);
            }
        }
        else
        {
            float dt = Time.deltaTime;

            playerPos = player.transform.position;
            pos = childEnemy.transform.position;

            switch (state)
            {
                case State.PATROL:
                    {

                        if (chasePlayer)
                        {
                            if (!audioSource.isPlaying)
                            {
                                audioSource.PlayOneShot(chase);
                            }
                            state = State.CHASE;

                            target = playerPos;

                            break;
                        }

                        if ((pos.x - target.x) < 0.2f && (pos.x - target.x) > -0.2f && (pos.y - target.y) < 0.2f && (pos.y - target.y) > -0.2f)
                        {
                            target = GetNextWaypoint().transform.position;
                        }
                    }
                    break;
                case State.CHASE:
                    {
                        Vector3 dir = playerPos - pos;

                        if (chasePlayer)
                        {
                            target = playerPos;
                            break;
                        }

                        state = State.PATROL;
                    }
                    break;
            }

            if (target.x > pos.x)
            {
                sr.flipX = false;
            }
            else if (target.x < pos.x)
            {
                sr.flipX = true;
            }

            if (pos != target)
            {
                if ((pos.x - player.transform.position.x) < 1f && (pos.x - player.transform.position.x) > -1f && (pos.y - player.transform.position.y) < 1f && (pos.y - player.transform.position.y) > -1f)
                {
                    attackPlayer = true;
                    anim.SetInteger("FighterAnim", 2);
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(attack);
                    }
                }
                else
                {
                    attackPlayer = false;
                    anim.SetInteger("FighterAnim", 1);
                }


                chaseDir = target - pos;
                chaseDir.Normalize();

                float angle = Mathf.Acos(Vector3.Dot(chaseDir, orientation) / (Vector3.Magnitude(chaseDir) * Vector3.Magnitude(orientation)));
                float maxAngle = maxAngularSpeed * dt;

                Vector3 normalVec;
                normalVec.x = orientation.y;
                normalVec.y = -orientation.x;
                normalVec.z = 0.0f;
                normalVec.Normalize();

                if (Vector3.Dot(normalVec, chaseDir) > 0)
                    maxAngle *= -1;

                Vector3 finalVel;
                finalVel.x = orientation.x * (Mathf.Cos(maxAngle)) - orientation.y * (Mathf.Sin(maxAngle));
                finalVel.y = orientation.x * (Mathf.Sin(maxAngle)) + orientation.y * (Mathf.Cos(maxAngle));
                finalVel.z = 0;

                orientation = finalVel.normalized;

                chaseVel = orientation.normalized * speed;
                pos = pos + (chaseVel * dt);
                childEnemy.transform.position = pos;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!childEnemy.GetComponent<EnemyAttack>().deadPlayer)
        {
            if (collision.gameObject.CompareTag("Player") && playerInArea == false)
            {
                chasePlayer = true;
                playerInArea = true;
            }
        }
        else
        {
            chasePlayer = false;
            playerInArea = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerInArea == true)
        {
            chasePlayer = false;
            playerInArea = false;
        }
    }
}
