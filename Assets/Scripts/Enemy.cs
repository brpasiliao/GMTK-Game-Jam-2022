using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public float speed;
    public Vector2 rightEnd;
    public Vector2 leftEnd;
    public bool goingToRight = true;

    void Update() {
        if (goingToRight) {
            if (transform.position.x < rightEnd.x)
                transform.position = Vector2.MoveTowards(transform.position, rightEnd, speed);
            else goingToRight = false;

        } else {
            if (transform.position.x > leftEnd.x)
                transform.position = Vector2.MoveTowards(transform.position, leftEnd, speed);
            else goingToRight = true;
        }
    }
}