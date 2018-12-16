using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour {
    
    [SerializeField] protected float hp;
	[SerializeField] protected float coinQty;
	[SerializeField] protected float damage;
	[SerializeField] protected float speed;
    protected Vector2 direction;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidBody2d;
    protected BoxCollider2D boxCollider;

    protected float gravity;
	[SerializeField] protected float jumpVelocity;
    protected float yVelocity;
    protected float distToGround;
    protected bool isRunning;
    protected bool invunerable;
	protected bool hitted;

    // Use this for initialization
    public virtual void Start () {
        rigidBody2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = transform.Find("Visuals").GetComponent<Animator>();
        spriteRenderer = transform.Find("Visuals").GetComponent<SpriteRenderer>();

		distToGround = boxCollider.bounds.extents.y;
		gravity = -0.5f;
		rigidBody2d.gravityScale = 0;
		hitted = false;
    }

    void FixedUpdate()
    {
        rigidBody2d.velocity = direction;
        direction = Vector2.zero;
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
		if (damage > 0) {
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
						hitted = true;
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
					if (!invunerable) {
						StartCoroutine(Flash(1f, 0.05f));
					}
				}
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

		if (transform.name == "Player") {
			animator.Play (transform.name + "Death");
			yield return new WaitForSeconds (1.5f);
			GetComponentInChildren<SpriteRenderer>().enabled = false;
			GetComponent<Collider2D> ().enabled = false;
		} else {
			print (transform.name);
			GameObject destroyExplosion = Instantiate (Resources.Load<GameObject> ("Prefabs/ExplosionDestroy"), new Vector2(transform.position.x,transform.position.y+1f), Quaternion.identity);
			spriteRenderer.enabled = false;
			GetComponent<Collider2D> ().enabled = false;

			Coin coin = Resources.Load<Coin> ("Prefabs/Coin");
			for (int i = 0; i < coinQty; i++) {
				Instantiate (coin, transform.position, Quaternion.identity);
			}

			yield return new WaitForSeconds (0.5f);
			Destroy(gameObject);
			Destroy(destroyExplosion);
		}
    }

    IEnumerator Flash(float time, float intervalTime)
    {
        float elapsedTime = 0f;
        int index = 0;
        
        animator.Play(transform.name + "Hurt");
		invunerable = true;
        while (elapsedTime < time)
        {
			if (hp <= 0)
				break;
            spriteRenderer.enabled = !spriteRenderer.enabled;

            elapsedTime += Time.deltaTime + intervalTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
		if (hp>0) 
			spriteRenderer.enabled = true;
		else
			spriteRenderer.enabled = false;
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
