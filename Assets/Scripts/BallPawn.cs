//Course: GPE104
//Prof: Matthew Henry
//Student: Chad V


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallPawn: Pawn
{
    public float moveSpeed = 3;
    public float turnSpeed = 90;
    public Transform tfBase;
    public Transform tfTurrent;
    private Vector2 velocity;
    public float ballSpeed = 5f;
    public bool isMoving = false;
    public Rigidbody2D rb;
    public float accel = 2.5f;
    public float maxSpeed = 15f;
    public float paddleInfluence = 0.2f; // 20% of paddle velocity
    public float paddleVelocity;


    public override void Move(Vector3 someVec3)
    {
        if (isMoving)
        {
            //tfBase.position += (Vector3)velocity * ballSpeed * Time.deltaTime;
        }
    }

    public override void Shoot(bool someBool)
    {
        
    }

    // rotate the turret by some angle
    public override void Rotate(float someAngle)
    {
        tfTurrent.Rotate(0,0,someAngle * turnSpeed * Time.deltaTime);
    }

    // on start get ref to Transform and Shooter.
    public override void Start()
    {
        Debug.Log("BallPawn.Start() - gameObject.name: " + gameObject.name);
        rb = GetComponent<Rigidbody2D>();
        tfBase = transform.Find("Ball");
        LaunchBall(new Vector3(Random.Range(-2f, 2f), -1f));
    }


    void FixedUpdate()
    {
        if (rb != null && isMoving && rb.linearVelocity.magnitude > maxSpeed){
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }


    public void LaunchBall(Vector2 launchDirection)
    {
        if (rb != null)
        {
            Vector2 launchVelocity = launchDirection.normalized * ballSpeed;
            
            // Clamp to maxSpeed
            if (launchVelocity.magnitude > maxSpeed)
            {
                launchVelocity = launchVelocity.normalized * maxSpeed;
            }
            
            rb.linearVelocity = launchVelocity;
            isMoving = true;
            Debug.Log("Ball launched! Velocity: " + rb.linearVelocity + ", magnitude: " + rb.linearVelocity.magnitude);
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on BallPawn!");
        }
    }

    public void BounceOffHorizontalSurface()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(-currentVelocity.x, currentVelocity.y);
        
        // Clamp to maxSpeed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    public void BounceOffVerticalSurface()
    {      
        Vector2 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(currentVelocity.x, -currentVelocity.y);
        
        // Clamp to maxSpeed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    public void BounceOffPaddle(float hitPosition, Vector2 paddleVelocity)
    {
        // Increase speed
        ballSpeed += accel;
        if (ballSpeed > maxSpeed)
        {
            ballSpeed = maxSpeed;
        }

        // Calculate angle-based velocity using the clamped ballSpeed
        float angle = hitPosition * 60f * (Mathf.PI / 180f);
        float xVel = Mathf.Sin(angle) * ballSpeed;
        float yVel = Mathf.Abs(Mathf.Cos(angle)) * ballSpeed;

        // Create new velocity vector
        Vector2 newVelocity = new Vector2(xVel, yVel);

        // Add paddle influence (small amount for realism)
        newVelocity += paddleVelocity * paddleInfluence;

        // Clamp to maxSpeed before applying
        if (newVelocity.magnitude > maxSpeed)
        {
            newVelocity = newVelocity.normalized * maxSpeed;
        }

        // Apply to physics body
        rb.linearVelocity = newVelocity;

        // Update ballSpeed to match actual speed for future calculations
        ballSpeed = rb.linearVelocity.magnitude;

        Debug.Log($"Paddle hit! ballSpeed={ballSpeed:F2}, velocity.magnitude={rb.linearVelocity.magnitude:F2}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the ball
        if (other == rb.GetComponent<Collider2D>())
        {
            Debug.Log("TopPushZone triggered - pushing ball down");
            
            // Push the ball downward with a gentle force
            // This prevents the ball from getting stuck at the top
            Vector2 pushForce = new Vector2(0, -2f); // Push straight down
            rb.AddForce(pushForce, ForceMode2D.Impulse);
            
            // Optional: Also ensure it's moving downward
            if (rb.linearVelocity.y > -2f) // If moving up or barely moving down
            {
                Vector2 currentVelocity = rb.linearVelocity;
                rb.linearVelocity = new Vector2(currentVelocity.x, -3f); // Ensure downward movement
            }
        }
    }
}
