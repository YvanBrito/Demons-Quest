using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	private Rigidbody2D rb;
	private Animator animator;
	private string[] coinType = new string[3] {"coinBronze_0", "coinSilver_0", "coinGold_0"};
	private int value;

	void Start () {
		int cType = Random.Range (0, coinType.Length);
		animator = GetComponent<Animator> ();
		RuntimeAnimatorController newController = Resources.Load<RuntimeAnimatorController>("CollectableAnimations/" + coinType[cType]);
		animator.runtimeAnimatorController = newController;

		switch (cType) {
		case 0:
			transform.localScale *= 0.8f;
			value = 1;
			break;
		case 1:
			transform.localScale *= 1.15f;
			value = 5;
			break;
		default:
			transform.localScale *= 1.25f;
			value = 10;
			break;
		}

		rb = GetComponent<Rigidbody2D> ();
		rb.AddForce (new Vector2(Random.Range(-50, 50), 400));
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			Physics2D.IgnoreCollision (GetComponent<Collider2D>(), coll.collider);
			coll.gameObject.SendMessage("UpScore", value);
			Destroy (gameObject);
		}
	}
}
