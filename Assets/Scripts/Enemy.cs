using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public override void Start()
    {
        base.Start();
    }

	public void Update(){
		if (IsGrounded ()) {
			yVelocity = 0;
		} else {
			yVelocity += gravity;
		}

		if (hp > 0)
		{
			transform.Translate(new Vector2(0, yVelocity * Time.deltaTime));
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
        {
            collision.transform.GetComponent<Player>().Hit(damage);
        }
    }
}
