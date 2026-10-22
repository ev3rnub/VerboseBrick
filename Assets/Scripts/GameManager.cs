//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; // for List<>
using TMPro;
using System;

public class GameManager : MonoBehaviour
{   
    [Header("GameObjects")]
    public static GameManager someGameManager;
    public ControllerBall controllerBall; // get controller ref for player entity.//
    public Health targetHealth;
    
    [Header("Spawn Slot locations")]
    private Vector3 spawnSlot0 = new Vector3(-6.38f, 3.92f, 0f); // far left
    private Vector3 spawnSlot1 = new Vector3(-2.63f, 3.92f, 0f);
    private Vector3 spawnSlot2 = new Vector3(-0.2449999f, 3.92f, 0f);
    private Vector3 spawnSlot3 = new Vector3(4.12f, 3.92f, 0f);
    private Vector3 spawnSlot4 = new Vector3(7.495f, 3.92f, 0f); // far right

    void Start()
    {
    }

    public void Awake()
    {

        if (someGameManager == null)
        {
            someGameManager = this;
            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SomeGameObjectDied(GameObject someGameObject)
    {
        Debug.Log($"RECEIVED {someGameObject.name}");
        //some delay before spawning to give player warning
        // then spawn ball
        //then spawn new block
        SpawnEnemy(1, someGameObject);
    }

    public void BrickHit()
    {
        //some fx
        //some sfx
        //somelogic

        Debug.Log("BOOM");
        // eventually this goes to a health manager but gotta start somewhere.
        var someInt = targetHealth.SubHealth(10);
        Debug.Log($"{someInt}");
        if (someInt <= 0)
        {

        }
    }

    public void BottomHit()
    {
        Debug.Log("BOTTOM");
    }

    public void SpawnEnemy(int someAmt, GameObject someGameObject)
    {
        var someRandInt = UnityEngine.Random.Range(0, 5);
        switch (someRandInt)
        {
            case 0:
            {
                Debug.Log(someRandInt);
                //spawn someGameObject.
                Vector3 somePos = spawnSlot0;
                Instantiate(someGameObject, somePos, Quaternion.identity); 
                targetHealth = someGameObject.GetComponent<Health>();
                targetHealth.AddHealth(100);
                break;
            }
            case 1:
            {
                Debug.Log(someRandInt);
                //spawn someGameObject.
                Vector3 somePos = spawnSlot1;
                Instantiate(someGameObject, somePos, Quaternion.identity); 
                targetHealth = someGameObject.GetComponent<Health>();
                targetHealth.AddHealth(100);
                break;
            }
            case 2:
            {
                Debug.Log(someRandInt);
                //spawn someGameObject.
                Vector3 somePos = spawnSlot2;
                Instantiate(someGameObject, somePos, Quaternion.identity);
                targetHealth = someGameObject.GetComponent<Health>();
                targetHealth.AddHealth(100);
                break;
            }
            case 3:
            {
                Debug.Log(someRandInt);
                //spawn someGameObject.
                Vector3 somePos = spawnSlot3;
                Instantiate(someGameObject, somePos, Quaternion.identity);
                targetHealth = someGameObject.GetComponent<Health>();
                targetHealth.AddHealth(100);
                break;
            }
            case 4:
            {
                Debug.Log(someRandInt);
                //spawn someGameObject.
                Vector3 somePos = spawnSlot4;
                Instantiate(someGameObject, somePos, Quaternion.identity);
                targetHealth = someGameObject.GetComponent<Health>();
                targetHealth.AddHealth(100);
                break;
            }
            default:
            {
                Debug.Log(someRandInt);
                break;
            }
        } 
    }
}