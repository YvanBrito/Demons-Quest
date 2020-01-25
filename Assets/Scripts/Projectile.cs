using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private bool isDestructible = true;

    private SpriteRenderer spriteRenderer;
    public float speed;
    public float damage;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = transform.Find("Visuals").GetComponent<SpriteRenderer>();
        //StartCoroutine(destroy());
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.flipX)
            transform.Translate(new Vector2(-speed * Time.deltaTime, 0));
        else
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        //print(Camera.main.WorldToScreenPoint(transform.position).x);
        if (Camera.main.WorldToScreenPoint(transform.position).x > Screen.width || Camera.main.WorldToScreenPoint(transform.position).x < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().Hit(damage);
        }
        else if (collision.GetComponent<Jumper>() != null)
        {
            collision.GetComponent<Jumper>().Hit(damage);
        }

        if (isDestructible)
            Destroy(gameObject);
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
