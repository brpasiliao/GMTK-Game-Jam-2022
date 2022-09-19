using UnityEngine;
using System.Collections;

public class Turtle : Player {
    public float speed;
    public float speedDash;
    public float dashTime;
    float dashTimer = 0;
    public float speedBoost;
    
    public float slowDown; // natural slowdown
    public float slowDown2; // forced slowdown
    public float chargeTime;
    float chargeTimer = 0;
    public float coolDown;
    float coolDownTimer;

    private bool isDashing = false;
    private bool isSliding = false;
    private bool isCharging = false;
    public bool enemyBoost;
    private bool isBoosted = false;
    private int direction = 1;

    // public AudioSource sfx;

    private void OnEnable() {
        currentSpeed = speed;
        currentJump = 0;

        GetComponent<Animator>().Play("Turtle_Idle");
    }

    protected override void Move() {
        if (!(isDashing || isSliding)) {
            if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                if (!isHit) facing = -1;
                moveVelocity = -currentSpeed;
                direction = -1;
            }
            if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                if (!isHit) facing = 1;
                moveVelocity = currentSpeed;
                direction = 1;
            }

        } else {
            if (isSliding) {
                if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
                    if (direction == 1) slowDown *= slowDown2;
                    else slowDown = 0.05f;
                } 
                if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
                    if (direction == -1) slowDown *= slowDown2;
                    else slowDown = 0.05f;
                }
                if (Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A) || 
                    Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D))
                    slowDown = 0.1f;
            }
        }

        if (isHit) moveVelocity = -hurtSpeed * facing;
    }
    
    protected override void Special() {
        if (!(isDashing || isSliding)) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                isCharging = true;
                chargeTimer = 0;
                currentSpeed = 0;
                // GetComponent<SpriteRenderer>().enabled = false;
                // sfx.Play();
            }

            if (Input.GetKey(KeyCode.Space)) {
                chargeTimer += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                isCharging = false;
                // sfx.Stop();

                if (chargeTimer > chargeTime) isDashing = true;
                else ResetValues();
            }
        }

        if (isDashing) {
            if (dashTimer < dashTime) {
                currentSpeed = speedDash;
                if (isBoosted) {
                    currentSpeed += speedBoost;
                    isBoosted = false;
                }
                moveVelocity = currentSpeed * direction;
                dashTimer += Time.deltaTime;
            } else {
                isDashing = false;
                dashTimer = 0;
                isSliding = true;
            }
        }

        if (isSliding) {
            if (currentSpeed > 1f) {
                if (isBoosted) {
                    currentSpeed += speedBoost;
                    isBoosted = false;
                }
                currentSpeed -= slowDown;
                moveVelocity = currentSpeed * direction;
            } else {
                if (coolDownTimer < coolDown) {
                    currentSpeed = 0;
                    coolDownTimer += Time.deltaTime;
                } else {
                    // player.gameObject.GetComponent<Animator>().Play("Shell_Out"); //player
                    // GetComponent<SpriteRenderer>().enabled = true;
                    coolDownTimer = 0f;
                    isSliding = false;
                    currentSpeed = speed;
                    slowDown = 0.1f;
                }
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision) {
        if (this.enabled) {
            base.OnCollisionEnter2D(collision);

            if (collision.gameObject.tag == "Wall" && (isDashing || isSliding))
                direction = -direction;
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision) {
        if (this.enabled) {
            base.OnCollisionExit2D(collision);

            if (rb.velocity.y > 0f) {
                if (collision.gameObject.tag == "Thirty") 
                    rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.x * direction * 0.58f); // tan(30)
                if (collision.gameObject.tag == "Forty Five") 
                    rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.x * direction); // tan(45)
                if (collision.gameObject.tag == "Ninety") {
                    rb.velocity = new Vector2 (0, rb.velocity.x * direction);
                    currentSpeed = 0;
                }
            }
        }
    }

    protected override void EnemyContact(Enemy enemy) {
        if (this.enabled) {
            if (isDashing || isSliding) {
                // enemy.CallDieTemporarily();
                enemy.StartCoroutine("DieTemporarily");
                if (enemyBoost) isBoosted = true;
            }
            else if (!isSafe) GetHurt(enemy);
        }
    }

    protected override void ResetValues() {
        isDashing = false;
        isSliding = false;

        dashTimer = 0f;
        coolDownTimer = 0f;
        slowDown = 0.1f;
        currentSpeed = speed;

        // GetComponent<SpriteRenderer>().enabled = true;
    }
}