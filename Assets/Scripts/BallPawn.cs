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
    public float maxSpeed = 45f;
    public float paddleInfluence = 0.4f; // 20% of paddle velocity
    public float paddleVelocity;
    public AudioSource hitAudioSource;
    public AudioSource hitAudioSourceII;


    public override void Move(Vector3 someVec3)
    {
        if (isMoving)
        {
            //tfBase.position += (Vector3)velocity * ballSpeed * Time.deltaTime;
        }
    }

    public override void Shoot()
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
        // activate particle system on bullet hit from player/self.
        ParticleSystem[] someVFX = this.gameObject.GetComponentsInChildren<ParticleSystem>();
        if (someVFX == null)
        {
            Debug.Log("SOMEVFX is NULL");
        }
        else
        {
            foreach( ParticleSystem somePS in someVFX)
            {
                // Get the Main module of the particle system
                var mainModule = somePS.main;
                
                // Set a random start rotation (in radians, from 0 to 360 degrees converted to radians)
                float randomAngleRad = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
                mainModule.startRotation = randomAngleRad;
                somePS.Play();
            }
            
        }
        hitAudioSourceII.Play();
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
        ParticleSystem[] someVFX = this.gameObject.GetComponentsInChildren<ParticleSystem>();
        if (someVFX == null)
        {
            Debug.Log("SOMEVFX is NULL");
        }
        else
        {
            foreach( ParticleSystem somePS in someVFX)
            {
                somePS.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                somePS.Play();
            }
            
        }        
        hitAudioSourceII.Play();
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
        //newVelocity += paddleVelocity * paddleInfluence;

        // Clamp to maxSpeed before applying
        if (newVelocity.magnitude > maxSpeed)
        {
            newVelocity = newVelocity.normalized * maxSpeed;
        }

        // Apply to physics body
        rb.linearVelocity = newVelocity;

        ballSpeed = rb.linearVelocity.magnitude;

        ParticleSystem[] someVFX = this.gameObject.GetComponentsInChildren<ParticleSystem>();
        if (someVFX == null)
        {
            Debug.Log("SOMEVFX is NULL");
        }
        else
        {
            foreach( ParticleSystem somePS in someVFX)
            {
                somePS.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                somePS.Play();
            }
            
        }
        hitAudioSource.Play();
        Debug.Log($"Paddle hit! ballSpeed={ballSpeed:F2}, velocity.magnitude={rb.linearVelocity.magnitude:F2}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the ball

    }
}
