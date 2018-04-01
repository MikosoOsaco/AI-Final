using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public GameObject hitbox;
    public bool isDead;
    public bool hasDeathEnded;
    public bool deadPlayer;

    public int health;

    private void Start()
    {
        health = 2;
        isDead = false;
        hasDeathEnded = false;
        deadPlayer = false;
    }

    public void AttackStart()
    {
        if (this.name == "Fighter")
        {

            if (this.GetComponent<SpriteRenderer>().flipX)
            {
                hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(-0.2f, 0);
            }
            else
            {
                hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            }
        }
        else if(this.name == "Knight")
        {
            if (this.GetComponent<SpriteRenderer>().flipX)
            {
                hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(-0.2f, 0);
            }
            else
            {
                hitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0.2f, 0);
            }
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
        if (collision.gameObject.CompareTag("Player"))
        {
            deadPlayer = true;
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.GetComponent<PlayerAttack>().isDead = true;
        }
    }
}
