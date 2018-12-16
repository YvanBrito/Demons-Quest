using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (player != null)
        {
            Vector2 target = new Vector2(player.position.x, player.position.y + 2);
            Vector3 newPos = Vector2.Lerp(transform.position, target, 2 * Time.deltaTime);
            newPos.x = Mathf.Clamp(newPos.x, 0, 46);
            newPos.y = Mathf.Clamp(newPos.y, -1, Mathf.Infinity);
            newPos.z = -10;

            transform.position = newPos;
        }
    }
}
