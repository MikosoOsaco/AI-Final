using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public GameObject hitbox;
    private Player playerScript;
    private AudioSource audioSource;
    public bool isDead;
    public bool hasDeathEnded;

    bool hasHit;

    private void Start()
    {
        playerScript = GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        isDead = false;
        hasDeathEnded = false;
        hasHit = false;
    }

    public void AttackStart()
    {
        if (playerScript.getFace() == Player.FACE.DOWN)
        {
            hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.1f);
            hitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.15f, 0.2f);
        }
        else if (playerScript.getFace() == Player.FACE.UP)
        {
            hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.1f);
            hitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.15f, 0.25f);
        }
        else if (playerScript.getFace() == Player.FACE.RIGHT)
        {
            hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0.1f, 0);
            hitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.15f);
        }
        else if (playerScript.getFace() == Player.FACE.LEFT)
        {
            hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(-0.1f, 0);
            hitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.25f, 0.15f);
        }
        hitbox.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void AttackEnd()
    {
        hitbox.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Die()
    {
        hitbox.GetComponent<BoxCollider2D>().enabled = false;
        hasDeathEnded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.name == "Fighter")
            {
                audioSource.Play();
                collision.GetComponent<EnemyAttack>().isDead = true;
            }
            else if (collision.name == "Knight")
            {
                audioSource.Play();
                if (hasHit)
                {
                    collision.GetComponent<EnemyAttack>().health--;
                    hasHit = false;
                }

                if (collision.GetComponent<EnemyAttack>().health <= 0)
                {
                    collision.GetComponent<EnemyAttack>().isDead = true;
                    collision.GetComponent<EnemyAttack>().health = 2;
                }
            }
            else if (collision.tag == "Ghost")
            {
                audioSource.Play();
                collision.GetComponent<Ghost>().isDead = true;
            }
            else if (collision.tag == "Bat")
            {
                audioSource.Play();
                collision.GetComponent<Bat>().isDead = true;
            }
            else if (collision.tag == "Finish")
            {
                GetComponent<Player>().canMove = false;
                GameObject.Find("WinCanvas").GetComponent<Canvas>().enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hasHit = true;
    }
}
