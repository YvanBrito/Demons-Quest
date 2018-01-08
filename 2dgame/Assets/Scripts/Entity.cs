using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour {
    
    [SerializeField] protected float hp;
    protected float damage;
    protected float speed;
    protected Vector2 direction;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidBody2d;
    protected BoxCollider2D boxCollider;

    protected float gravity;
    protected float jumpVelocity;
    protected float yVelocity;
    protected float distToGround;
    protected bool isRunning;
    protected bool invunerable;

    // Use this for initialization
    public virtual void Start () {
        rigidBody2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = transform.Find("Visuals").GetComponent<Animator>();
        spriteRenderer = transform.Find("Visuals").GetComponent<SpriteRenderer>();

        distToGround = boxCollider.bounds.extents.y;
        rigidBody2d.gravityScale = 0;
    }

    public virtual void Animation()
    {
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (direction.x != 0)
            isRunning = true;
        else
            isRunning = false;

        if (yVelocity > -0.5f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
        else if (yVelocity < -0.5f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        animator.SetBool("isRunning", isRunning);
    }

    public void Hit(float damage)
    {
        if (transform.name == "Player")
        {
            if (!invunerable)
            {
                hp -= damage;
                if (hp <= 0)
                {
                    StartCoroutine(Death());
                }
                else
                {
                    StartCoroutine(Flash(1f, 0.05f));
                }
            }
        }
        else
        {
            hp -= damage;
            if (hp <= 0)
            {
                StartCoroutine(Death());
            }
            else
            {
                if (!invunerable)
                    StartCoroutine(Flash(1f, 0.05f));
            }
        }
    }

    IEnumerator Death()
    {
        boxCollider.enabled = false;
        
        if (hp <= 0)
        {
            direction = Vector2.zero;
            transform.Translate(Vector2.zero);
        }

        animator.Play(transform.name + "Death");
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    IEnumerator Flash(float time, float intervalTime)
    {
        float elapsedTime = 0f;
        int index = 0;
        
        animator.Play(transform.name + "Hurt");
        invunerable = true;
        while (elapsedTime < time)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            elapsedTime += Time.deltaTime + intervalTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
        spriteRenderer.enabled = true;
        invunerable = false;
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 0.01f);
        if (hit)
        {
            if (hit.transform.tag == "Coin")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
