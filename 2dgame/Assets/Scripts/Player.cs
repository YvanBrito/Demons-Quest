using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Entity
{
    public Projectile fireblast;
    public Slider healthBar;
    public float amount;
	public Text scoreText;

    private Projectile projectile;
	private int scorePoints;

    bool flying, androidJump, androidAttack;

    public override void Start ()
    {
        base.Start();
        speed = 7.0f;
        jumpVelocity = 13;
		scorePoints = 0;

        hp = healthBar.maxValue = 10;
    }

    // Update is called once per frame
	void Update () {
        if (IsGrounded())
        {
            yVelocity = 0;
            flying = false;
        }

        if (jumpInputs() && IsGrounded())
        {
            print("Pulando");
            yVelocity = jumpVelocity;
        }

        if (!flying && jumpInputs() && !IsGrounded())
        {
            yVelocity = 0;
            flying = true;
        }else if (flying && jumpInputs())
        {
            flying = false;
        }

        if (!flying)
            yVelocity += gravity;

        if (hp > 0)
        {
            Walk();
            transform.Translate(new Vector2(0, yVelocity * Time.deltaTime));
        }

        androidJump = false;
        Animation();

        healthBar.value = hp;
		scoreText.text = "Score: " + scorePoints.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void FixedUpdate()
    {
        rigidBody2d.velocity = direction;
        direction = Vector2.zero;
    }

    public override void Animation()
    {
        base.Animation();
        if (attackInputs() && !projectile)
        {
            animator.SetBool("isAttacking", true);
            projectile = Instantiate(fireblast, transform.Find("FireblastGun").position, Quaternion.identity);
            projectile.transform.Find("Visuals").GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        }
        else
        {
            animator.SetBool("isAttacking", false);
            androidAttack = false;
        }
    }

	public void UpScore(int value){
		scorePoints += value;
	}

    #region Inputs
    public void Walk()
    {
        if (Input.GetAxis("Horizontal") == 0)
        {
            direction = new Vector2(amount * speed, 0);
        }
        else
        {
            direction = new Vector2(Input.GetAxis("Horizontal") * speed, 0);
        }
    }

    public bool attackInputs()
    {
        return (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 2") || androidAttack);
    }

    public bool jumpInputs()
    {
        return (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown("joystick button 0") || androidJump);
    }

    public void Attack()
    {
        androidAttack = true;
    }

    public void Jump()
    {
        androidJump = true;
    }
    #endregion
}
