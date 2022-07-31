using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public string form;
    public float speed;
    public float pause;
    public float timer = 0f;
    public Vector2 rightEnd;
    public Vector2 leftEnd;
    public bool goingToRight = true;

    void Update() {
        if (goingToRight) {
            if (transform.position.x < rightEnd.x)
                transform.position = Vector2.MoveTowards(transform.position, rightEnd, speed);
            else {
                if (timer < pause) timer += Time.deltaTime;
                else {
                    timer = 0f;
                    goingToRight = false;
                }
            }

        } else {
            if (transform.position.x > leftEnd.x)
                transform.position = Vector2.MoveTowards(transform.position, leftEnd, speed);
            else {
                if (timer < pause) timer += Time.deltaTime;
                else {
                    timer = 0f;
                    goingToRight = true;
                }
            }
        }
    }
}