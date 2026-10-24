//Course: GPE104
//Prof: Matthew Henry
//Student: Chad V

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class ControllerBall : MyController
{
    private Camera cam;
    public float someMaxWidth = Screen.width;
    public float ballSpeed = 135f;
    private float updateInterval = 60f; // Update every min
    public string moveDir = "";
    public GameManager someGameManager;


// like init, but called start!
    void Start()
    {
        cam = Camera.main; // get ref to main camera
        Debug.Log(cam);

        if (GameManager.someGameManager != null)
        {
           GameManager.someGameManager.controllerBall = this;
        }

        if (somePawn != null)
        {
            (somePawn as BallPawn).LaunchBall(new Vector2(Random.Range(-0.5f, 0.5f), -1f));
            Debug.Log("ControllerBall started and launched ball");
        }
        else
        {
            Debug.LogError("somePawn is null in ControllerBall!");
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //collisions NOT WORKING BOOO,
        Debug.Log("OnCollisionEnter2D fired with: " + collision.gameObject.name + " tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("WallLeft") || 
            collision.gameObject.CompareTag("WallRight"))
        {
            Debug.Log("WALL RIGHT/LEFT");
            (somePawn as BallPawn).BounceOffHorizontalSurface();
        }
        else if (collision.gameObject.CompareTag("WallTop"))
        {
            (somePawn as BallPawn).BounceOffVerticalSurface();
            Debug.Log("WALL TOP");
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            GameManager.someGameManager.Defend();
            Debug.Log("PADDLE");
            // Get paddle pawn
            PaddlePawn paddlePawn = collision.gameObject.GetComponent<PaddlePawn>();
            Vector2 paddleVelocity = Vector2.zero;
            if (paddlePawn != null)
            {
                paddlePawn.UpdatePaddleVelocity(); // Update velocity before getting it
                paddleVelocity = paddlePawn.paddleVelocity;
            }

            // get hit position relative to paddle center
            // get paddle collider size
            Collider2D paddleCollider = collision.collider;
            float paddleWidth = paddleCollider.bounds.size.x;
            
            // Calculate hit position: -1 (left edge) to 1 (right edge)
            float hitPosition = (collision.transform.position.x - transform.position.x) / (paddleWidth / 2f);
            
            // Clamp hit position between -1 and 1
            hitPosition = Mathf.Clamp(hitPosition, -1f, 1f);
            
            (somePawn as BallPawn).BounceOffPaddle(hitPosition, paddleVelocity);
        }
        else if (collision.gameObject.CompareTag("Brick"))
        {
            // GameManger.Instance....
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            someGameManager.BrickHit();
            (somePawn as BallPawn).BounceOffVerticalSurface();
        }
        else if (collision.gameObject.CompareTag("EnemyPaddle"))
        {
            // GameManger.Instance....Need to change this as I added enemyblock tag. 
            someGameManager.targetHealth = collision.gameObject.GetComponent<Health>();
            someGameManager.BrickHit();
            (somePawn as BallPawn).BounceOffVerticalSurface();            
        }
        else if (collision.gameObject.CompareTag("WallBot"))
        {
            // Ball lost - handle life loss
            someGameManager.BottomHit();
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("BULLET ENCOUNTERED");
        }
    }
}