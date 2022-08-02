using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public string form;

    public bool canWalk;
    public float speed;
    public float pause;
    float timer = 0f;

    public Vector2 rightEnd;
    public Vector2 leftEnd;
    public bool goingToRight = true;

    float ogScaleX;

    void Start() {
        ogScaleX = transform.localScale.x;
    }

    void Update() {
        if (canWalk) {
            if (goingToRight) {
                if (transform.position.x < rightEnd.x)
                    transform.position = Vector2.MoveTowards(transform.position, rightEnd, speed);
                else {
                    if (timer < pause) timer += Time.deltaTime;
                    else {
                        timer = 0f;
                        goingToRight = false;
                        transform.localScale = new Vector3(-ogScaleX, transform.localScale.y, transform.localScale.z);
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
                        transform.localScale = new Vector3(ogScaleX, transform.localScale.y, transform.localScale.z);
                    }
                }
            }
        }
    }
}