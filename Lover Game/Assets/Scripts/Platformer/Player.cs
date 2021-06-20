using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerController))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4f;
    public float minHumpHeight = 1f;
    public float timeToJumpApex = 0.4f;
    public float maxFallSpeed = 16f;
    public float minY;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrouded = 0.1f;
    float moveSpeed = 9;
    bool enableWallSliding = false;
    bool ignoreInput = false;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = 0.25f;
    float timeToWallUnstick;

    float gravity;
    float gravityMultiplier = 1f;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    float balloonGravityMultiplier = 0.01f;
    List<PlayerBalloon> balloons;

    PlatformerController controller;
    PlayerGun gun;

    [HideInInspector]
    public Vector3 checkpoint;

    Animator animator;
    SpriteRenderer sr;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    int directionX;
    float inputDirectionX = 1;
    bool dying = false;

    public static Player Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlatformerController>();
        gun = GetComponent<PlayerGun>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        checkpoint = transform.position;

        gravity = -2f * maxJumpHeight / (timeToJumpApex * timeToJumpApex);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minHumpHeight);

        gravity *= 2f; // compensate for floaty jumps

        balloons = new List<PlayerBalloon>();

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();
        if (enableWallSliding) HandleWallSliding();

        UpdateAnimator();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope) velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            else velocity.y = 0;
        }

        if (transform.position.y < minY) Respawn();
    }

    private void UpdateAnimator()
    {
        if (velocity.x > 0) sr.flipX = false;
        else if (velocity.x < 0) sr.flipX = true;

        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        directionX = (int)Mathf.Sign(directionalInput.x);

        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0f)
        {
            wallSliding = true;
            if (velocity.y < -wallSlideSpeedMax) velocity.y = -wallSlideSpeedMax;

            if (timeToWallUnstick > 0f)
            {
                velocity.x = 0f;
                velocityXSmoothing = 0f;

                if (directionX != wallDirX && directionalInput.x != 0f) timeToWallUnstick -= Time.deltaTime;
                else timeToWallUnstick = wallStickTime;
            }
            else timeToWallUnstick = wallStickTime;
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrouded : accelerationTimeAirborne);
        velocity.y += (gravityMultiplier - balloonGravityMultiplier * balloons.Count) * gravity * Time.deltaTime;
        if (velocity.y < -maxFallSpeed) velocity.y = -maxFallSpeed;
    }

    public void OnJumpInputDown()
    {
        gravityMultiplier = 0.5f;

        if (wallSliding)
        {
            animator.SetBool("IsJumping", true);
            animator.SetTrigger("Jump");
            if (wallDirX == directionX)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionX == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        else if (controller.collisions.below)
        {
            animator.SetBool("IsJumping", true);
            animator.SetTrigger("Jump");
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (Mathf.Sign(directionalInput.x) != -Mathf.Sign(controller.collisions.slopeNormal.x))
                {
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                }
            }
            else velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp()
    {
        gravityMultiplier = 1f;
        if (velocity.y > minJumpVelocity && balloons.Count == 0) velocity.y = minJumpVelocity;
    }

    public void OnLand()
    {
        gravityMultiplier = 1f;
        animator.SetBool("IsJumping", false);
    }

    public void OnFire1Down()
    {
        gun.Fire(Vector2.right * inputDirectionX);
    }

    public void OnFire2Down()
    {
        if (balloons.Count > 0) balloons[0].Pop(true);
    }

    public void SetDirectionalInput(Vector2 input)
    {
        if (!ignoreInput)
        {
            directionalInput = input;
            if (input.x != 0f) inputDirectionX = Mathf.Sign(input.x);
        }
    }

    public void Respawn()
    {
        if (!dying)
        {
            dying = true;
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        float fadeTime = 0.5f;
        float moveSpeed = 15f;
        Vector3 start;
        float distance = Vector3.Distance(transform.position, checkpoint);
        float t = 0f;
        animator.SetBool("IsDead", true);
        ignoreInput = true;
        directionalInput = Vector2.zero;

        while (balloons.Count > 0)
            balloons[0].Pop(false);

        // fade away
        while (t < 1f)
        {
            t += Time.deltaTime / fadeTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - 0.5f*Tween.GetEasedValue(t, Tween.Easing.InOutSine));

            yield return null;
        }

        // move to checkpoint
        start = transform.position;
        t = 0f;
        while (t < 1f)
        {
            t += (distance != 0) ?
                Time.deltaTime * moveSpeed / distance :
                1f;

            transform.position = Vector3.LerpUnclamped(start, checkpoint, Tween.GetEasedValue(t, Tween.Easing.InOutSine));

            yield return null;
        }

        animator.SetBool("IsDead", false);
        velocity = Vector3.zero;
        ignoreInput = false;
        UpdateAnimator();

        // fade in
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f + 0.5f*Tween.GetEasedValue(t, Tween.Easing.InOutSine));

            yield return null;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        dying = false;
    }

    public void RegisterBalloon(PlayerBalloon balloon)
    {
        balloons.Add(balloon);
    }

    public void DeregisterBalloon(PlayerBalloon balloon)
    {
        if (balloons.Contains(balloon)) balloons.Remove(balloon);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-5000f, minY, 0), new Vector3(5000f, minY, 0));
    }
}
