//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class ControllerPlayer : MyController
{
    private Camera cam; 

    public Key strafeLeft = Key.A;
    public Key strafeRight = Key.D;
    public Key rotateRight = Key.J;
    public Key rotateLeft = Key.L;
    public Key quitGame = Key.Escape;
    public Key shoot = Key.Space;
    public Key options = Key.O;

    public float someMaxWidth = Screen.width;

    public Vector3 right = new Vector3(1f, 0f, 0f);
    public Vector3 left = new Vector3(-1f, 0f, 0f);

    public AudioSource shootAudioSource;
    public AudioSource moveAudioSource;
    public AudioSource bulletHitAudioSource;
    public AudioSource ballHitAudioSource;
    // like init, but called start!
    void Start()
    {
        cam = Camera.main; // get ref to main camera
        Debug.Log(cam);

        if (GameManager.someGameManager != null)
        {
           GameManager.someGameManager.controlPaddle = this;
        }

       if (somePawn == null)
       {
           somePawn = GetComponent<PaddlePawn>(); // Get pawn from same GameObject
           // OR: somePawn = FindObjectOfType<PaddlePawn>(); // Find any pawn in scene
       }        
    }

    // called once per frame
    void Update()
    {

        if (somePawn != null)
        {
            // Check if either shift key is held
            bool solidHeld = Keyboard.current.leftShiftKey.isPressed
                          || Keyboard.current.rightShiftKey.isPressed;

            
            // check if a is being pressed.
            if (Keyboard.current[strafeLeft].isPressed)
            {
             //   Debug.Log("a was pressed!");
                // move sprite, etc.
                somePawn.Move(left);

            }
            // check if d is being pressed.
            if (Keyboard.current[strafeRight].isPressed)
            {
                // rotate.
                somePawn.Move(right);
            }

            if (Keyboard.current[rotateLeft].isPressed)
            {
                somePawn.Rotate(-1);
            }

            if (Keyboard.current[rotateRight].isPressed)
            {
                somePawn.Rotate(1);
            }

            if (Keyboard.current[shoot].wasPressedThisFrame)
            {
                somePawn.Shoot();
                GameManager.someGameManager.bulletsFired += 1;
                shootAudioSource.Play();
            }
            // check if escape was pressed.
            if (Keyboard.current[quitGame].isPressed)
            {
                //Debug.Log("ESCAPE was pressed!");
                Application.Quit();

                // If running inside the Unity Editor
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }

        }       
    }
}
