using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseOnSight : MonoBehaviour {

    private bool chasePlayer = false;
    private bool playerInArea = false;

    public GameObject player;
    public GameObject childEnemy;

    private Animator anim;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    public AudioClip attack;
    public AudioClip chase;

    private Vector3 pos;
    private Vector3 originalPos;
    private Vector3 target;
    private Vector3 chaseDir;
    private Vector3 chaseVel;

    private Vector3 orientation;
    private float speed;
    private float maxAngularSpeed;



    // Use this for initialization
    void Start () {
        //pos = this.gameObject.transform.GetChild(0).position;
        pos = childEnemy.transform.position;
        orientation = childEnemy.transform.up;
        originalPos = pos;
        speed = 3f;
        maxAngularSpeed = 5f;
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        sr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        anim.SetInteger("KnightAnim", 0);
    }
	
	// Update is called once per frame
	void Update () {

        if (childEnemy.GetComponent<EnemyAttack>().isDead)
        {
            childEnemy.GetComponent<BoxCollider2D>().enabled = false;
            anim.SetInteger("KnightAnim", 4);

            if (childEnemy.GetComponent<EnemyAttack>().hasDeathEnded)
            {
                sr.enabled = false;
                Destroy(this);
            }
        }
        else
        {
            float dt = Time.deltaTime;

            //Debug.DrawRay(childEnemy.transform.position, orientation, Color.red);

            RaycastHit2D lRAYhit = Physics2D.Raycast(childEnemy.transform.position, orientation, 1);

            if (!chasePlayer)
            {
                target = originalPos;

                if ((pos.x - originalPos.x) < 0.2f && (pos.x - originalPos.x) > -0.2f && (pos.y - originalPos.y) < 0.2f && (pos.y - originalPos.y) > -0.2f)
                {
                    pos = originalPos;
                }
            }
            else
            {
                target = player.transform.position;
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
                    anim.SetInteger("KnightAnim", 2);
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(attack);
                    }
                }
                else
                {
                    anim.SetInteger("KnightAnim", 1);
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
            else if (pos == target && target == originalPos)
            {
                anim.SetInteger("KnightAnim", 0);
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
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(chase);
                }
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
