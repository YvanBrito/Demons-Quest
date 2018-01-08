using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    public override void Start()
    {
        base.Start();

        hp = 3;
        damage = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Player")
        {
            collision.transform.GetComponent<Player>().Hit(damage);
        }
    }
}
