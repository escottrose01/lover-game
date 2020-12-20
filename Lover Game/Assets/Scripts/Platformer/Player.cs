using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlatformerController))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4f;
    public float minHumpHeight = 1f;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrouded = 0.1f;
    float moveSpeed = 6f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = 0.25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    PlatformerController controller;
    PlayerGun gun;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    int directionX;
    float inputDirectionX = 1;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlatformerController>();
        gun = GetComponent<PlayerGun>();

        gravity = -2f * maxJumpHeight / (timeToJumpApex * timeToJumpApex);
        maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minHumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope) velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            else velocity.y = 0;
        }
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
        velocity.y += gravity * Time.deltaTime;
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
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
        if (velocity.y > minJumpVelocity) velocity.y = minJumpVelocity;
    }

    public void OnFireDown()
    {
        gun.Fire(Vector2.right * inputDirectionX);
    }
    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
        if (input.x != 0f) inputDirectionX = Mathf.Sign(input.x);
    }
}
