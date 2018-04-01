using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    enum MOVEMENT { RIGHT, LEFT, DOWN, UP, NONE };
    public enum FACE { RIGHT, LEFT, DOWN, UP };
    public float speed = 1.0f;
    MOVEMENT moveState;
    FACE facing;
    Animator anim;
    SpriteRenderer sr;
    private AudioSource audioSource;
    public AudioClip swordSlash;
    public AudioClip deathSound;

    public bool canMove = true;
    float moveSpeed = 5f;
    float h;
    float v;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<PlayerAttack>().isDead)
        {
            PlayDeathSound();
            canMove = false;
            this.GetComponent<PlayerAttack>().hitbox.GetComponent<BoxCollider2D>().enabled = false;
            anim.SetBool("Dead", true);

            if (this.GetComponent<PlayerAttack>().hasDeathEnded)
            {
                sr.enabled = false;
                Destroy(this);
                GameObject.Find("LoseCanvas").GetComponent<Canvas>().enabled = true;
            }
        }
        else
        {
            float dt = Time.fixedDeltaTime;

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            if (h < 0)
            {
                moveState = MOVEMENT.LEFT;
                facing = FACE.LEFT;
                anim.SetBool("Left", true);
                anim.SetBool("Right", false);
                anim.SetBool("Down", false);
                anim.SetBool("Up", false);
                anim.SetBool("Idle", false);
            }
            else if (h > 0)
            {
                moveState = MOVEMENT.RIGHT;
                facing = FACE.RIGHT;
                anim.SetBool("Left", false);
                anim.SetBool("Right", true);
                anim.SetBool("Down", false);
                anim.SetBool("Up", false);
                anim.SetBool("Idle", false);
            }
            else if (v < 0)
            {
                moveState = MOVEMENT.DOWN;
                facing = FACE.DOWN;
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
                anim.SetBool("Down", true);
                anim.SetBool("Up", false);
                anim.SetBool("Idle", false);
            }
            else if (v > 0)
            {
                moveState = MOVEMENT.UP;
                facing = FACE.UP;
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
                anim.SetBool("Down", false);
                anim.SetBool("Up", true);
                anim.SetBool("Idle", false);
            }
            else
            {
                moveState = MOVEMENT.NONE;
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
                anim.SetBool("Down", false);
                anim.SetBool("Up", false);
                anim.SetBool("Idle", true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

                audioSource.PlayOneShot(swordSlash);
                switch (facing)
                {
                    case FACE.LEFT:
                        anim.SetTrigger("LeftAttack");
                        break;
                    case FACE.RIGHT:
                        anim.SetTrigger("RightAttack");
                        break;
                    case FACE.DOWN:
                        anim.SetTrigger("DownAttack");
                        break;
                    case FACE.UP:
                        anim.SetTrigger("UpAttack");
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void FixedUpdate()
    {
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

    public FACE getFace()
    {
        return facing;
    }

    private void PlayDeathSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
