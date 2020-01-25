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
	public float speedHit;
	public Text scoreText;

    private Projectile projectile;
	private int scorePoints;
	private AudioSource audioSource;

	bool flying, androidJump, androidAttack;
	bool playSoundFire, posHit;

    public override void Start ()
    {
        base.Start();
        speed = 7.0f;
		scorePoints = 0;

        hp = healthBar.maxValue = 10;

		audioSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
	void Update () {
        if (IsGrounded())
        {
            yVelocity = 0;
            flying = false;
			posHit = false;
        }

        if (jumpInputs() && IsGrounded())
        {
            Jump();
        }

		if (!flying && jumpInputs() && !IsGrounded() && !posHit)
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
			if (hitted) {
				posHit = true;
				flying = false;
				yVelocity = jumpVelocity/2;
				StartCoroutine (hitVelocity(spriteRenderer.flipX));

				hitted = false;
			}

			if (!posHit) {
				Walk ();
			}

			transform.Translate(new Vector2(speedHit * Time.deltaTime, yVelocity * Time.deltaTime));
        }

        androidJump = false;
        Animation();

        healthBar.value = hp;
		scoreText.text = "Score: " + scorePoints.ToString();

		PlayAudios ();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

	IEnumerator hitVelocity(bool left){
		if (left) {
			speedHit = 2;
		} else {
			speedHit = -2;
		}
		yield return new WaitForSeconds(0.8f);
		speedHit = 0;
	}

	void PlayAudios(){
		if (flying) {
			audioSource.clip = Resources.Load<AudioClip> ("SFX/WingFlap");
			audioSource.loop = true;
			audioSource.volume = 0.6f;
			if (!audioSource.isPlaying)
				audioSource.Play ();
		} else if (jumpInputs () && !posHit && hp>0) {
            audioSource.clip = Resources.Load<AudioClip> ("SFX/Jumping");
			audioSource.loop = false;
			audioSource.volume = 0.6f;
			audioSource.Play ();
		} else if (playSoundFire) {
            audioSource.clip = Resources.Load<AudioClip> ("SFX/Fire");
			audioSource.loop = false;
			audioSource.volume = 0.6f;
			audioSource.Play ();
		}else if(audioSource.loop){
			audioSource.Stop ();
		}
	}

    public override void Animation()
    {
        base.Animation();
		if (attackInputs() && !projectile && !posHit && hp>0)
        {
			playSoundFire = true;
            animator.SetBool("isAttacking", true);
            projectile = Instantiate(fireblast, transform.Find("FireblastGun").position, Quaternion.identity);
            projectile.transform.Find("Visuals").GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        }
        else
        {
            animator.SetBool("isAttacking", false);
			androidAttack = playSoundFire = false;
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

    public void JumpAndroid()
    {
        androidJump = true;
    }
    #endregion
}
