using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : Entity
{
    public Transform player;

    Vector3 initialPos;
    float spd = 0;
    float lastJump = .0f;
    float lastAttack = .0f;
    int jumpsToFlip = 0;
    int jumpToAttackCounter = 0;
    int jumpsAttackLimit = 0;
    int numAttacks = 0;
    int numAttackLimit = 0;

    bool isAttacking = false;

    public override void Start()
    {
        base.Start();
        speed = 1.5f;
        initialPos = transform.position;

        numAttackLimit = Random.Range(2, 5);
        jumpsAttackLimit = Random.Range(3, 6);
        print(numAttackLimit + " | " + jumpsAttackLimit);
    }

    public void Update()
    {
        if (IsGrounded())
        {
            yVelocity = 0;
        }
        else
        {
            yVelocity += gravity;
        }

        if (hp > 0)
        {
            animator.SetBool("isGrounded", IsGrounded());
            if (IsGrounded())
            {
                if (jumpToAttackCounter >= jumpsAttackLimit)
                {
                    if (numAttacks >= numAttackLimit && !animator.GetBool("isAttacking"))
                    {
                        if (Time.time > lastAttack + 1f)
                        {
                            numAttackLimit = Random.Range(2, 5);
                            jumpsAttackLimit = Random.Range(3, 6);
                            jumpToAttackCounter = 0;
                        }
                    }
                    else
                    {
                        if ((player.position.x < transform.position.x && spriteRenderer.flipX) || (player.position.x >= transform.position.x && !spriteRenderer.flipX))
                        {
                            Jump();
                        }
                        else
                        {
                            Attack();
                        }
                    }
                }
                else
                {
                    Jump();
                    numAttacks = 0;
                }
            }
            else
            {
                if (jumpsToFlip > 0)
                {
                    speed = -1 * speed;
                    jumpsToFlip = 0;
                }
                direction = new Vector2(speed, 0);
            }
            Animation();
            transform.Translate(new Vector2(0, yVelocity * Time.deltaTime));
        }
    }
    private void Attack()
    {
        if (Time.time > lastAttack + .5f)
        {
            animator.SetBool("isAttacking", true);
        }
    }
    private void Jump()
    {
        if (Time.time > lastJump + 1f)
        {
            base.Jump();
            lastJump = Time.time;
            jumpsToFlip++;
            jumpToAttackCounter++;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
        {
            collision.transform.GetComponent<Player>().Hit(damage);
        }
    }

    public void StopAttacking()
    {
        animator.SetBool("isAttacking", false);
        lastAttack = Time.time;
        numAttacks++;
    }
}
