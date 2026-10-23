//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class ControllerEnemy : MyController
{
    private Camera cam;

    public float someMaxWidth = Screen.width;

    public Vector3 right = new Vector3(1f, 0f, 0f);
    public Vector3 left = new Vector3(-1f, 0f, 0f);
    private float updateInterval = 3f; // Update every 1 second
    public string moveDir = "";

    // like init, but called start!
    void Start()
    {
        cam = Camera.main; // get ref to main camera
        Debug.Log(cam);

        //if (GameManager.someGameManager != null)
        //{
        //    GameManager.someGameManager.controllerEnemy = this;
        //}

       if (somePawn == null)
       {
           somePawn = GetComponent<EnemyPawn>(); // Get pawn from same GameObject
       }

       if (somePawn != null)
       {
        StartCoroutine(START_BLOCKAI());
       }
    }

    // Coroutine to update debug messages every second
    private IEnumerator START_BLOCKAI()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);

            MoveRandom();
        }
    }

    void Update()
    {
        if(moveDir != null)
        {
            if (moveDir == "RIGHT")
            {
                somePawn.Move(right);
            }
            else
            {
                somePawn.Move(left);
            }
            
        }
    }

    void MoveRandom()
    {
        var someInt = UnityEngine.Random.Range(0, 10);
        Debug.Log("ALICE, " + someInt);
        
        // Determine the randomly chosen direction
        Vector3 chosenDirection = (someInt <= 5) ? right : left;
        if (chosenDirection.x == 1)
        {
            moveDir = "RIGHT";
        }
        else
        {
            moveDir = "LEFT";
        }
    }

}
