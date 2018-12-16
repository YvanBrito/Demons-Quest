using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    Vector3 initialPos;

    public override void Start()
    {
        base.Start();
        speed = 1.5f;
        initialPos = transform.position;
    }

	public void Update(){
		if (IsGrounded ()) {
			yVelocity = 0;
		} else {
			yVelocity += gravity;
		}

		if (hp > 0)
		{
            if (transform.position.x > initialPos.x - 3 && !spriteRenderer.flipX)
            {
                direction = new Vector2(-speed, 0);
            }
            else if (transform.position.x < initialPos.x + 3 && spriteRenderer.flipX)
            {
                direction = new Vector2(speed, 0);
            }
            else
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }

			transform.Translate(new Vector2(0, yVelocity * Time.deltaTime));
		}
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
        {
            collision.transform.GetComponent<Player>().Hit(damage);
        }
    }
}
